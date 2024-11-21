using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Features.Commands.UpdateNodeInformation;
using TreeService.Application.Interfaces;
using TreeService.Domain;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Tests;

public class UpdateNodeInformationHandlerTests
{
    private static readonly Mock<IValidator<UpdateNodeInformationCommand>> _mockValidator = new();
    private static readonly Mock<ITreeRepository> _mockTreeRepository = new();
    private static readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private static readonly Mock<ILogger<CreateNodeHandler>> _mockLogger = new();

    private readonly UpdateNodeInformationHandler _handler = new UpdateNodeInformationHandler(_mockValidator.Object,
        _mockTreeRepository.Object,
        _mockUnitOfWork.Object,
        _mockLogger.Object);


    /// <summary>
    /// Проверяет успешное обновление информации узла.
    /// </summary>
    [Fact(DisplayName = "UpdateNodeInformationHandler updates node successfully")]
    public async Task Execute_ValidCommand_ShouldUpdateNode()
    {
        // Arrange
        var command = new UpdateNodeInformationCommand(Guid.NewGuid(), "Updated Title", "Updated Description");
        var existingNode = Node
            .Create(Guid.NewGuid(), Title.Create("title").Value, Description.Create("description").Value)
            .Value;

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockTreeRepository
            .Setup(repo => repo.GetById(command.NodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<Node, Error>(existingNode));

        _mockUnitOfWork
            .Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Execute(command);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет поведение при ошибке валидации.
    /// </summary>
    [Fact(DisplayName = "UpdateNodeInformationHandler returns validation errors")]
    public async Task Execute_InvalidCommand_ShouldReturnValidationErrors()
    {
        // Arrange
        var command = new UpdateNodeInformationCommand(Guid.NewGuid(), "", "");
        var validationFailures = new List<ValidationFailure>
        {
            new(nameof(command.Title), Errors.Node.ValueIsRequired("title").Serialize()),
        };
        var validationResult = new ValidationResult(validationFailures);

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Execute(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.Message == $"The title is required.");
    }

    /// <summary>
    /// Проверяет поведение при отсутствии узла.
    /// </summary>
    [Fact(DisplayName = "UpdateNodeInformationHandler returns error when node not found")]
    public async Task Execute_NodeNotFound_ShouldReturnError()
    {
        // Arrange
        var command = new UpdateNodeInformationCommand(Guid.NewGuid(), "Updated Title", "Updated Description");

        _mockValidator
            .Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        _mockTreeRepository
            .Setup(repo => repo.GetById(command.NodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Node, Error>(Errors.Node.NotFound(command.NodeId)));

        // Act
        var result = await _handler.Execute(command);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.Message.Contains("not found"));
    }
}
