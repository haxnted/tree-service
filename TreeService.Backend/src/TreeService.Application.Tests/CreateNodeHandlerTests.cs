using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Moq;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Interfaces;
using TreeService.Domain;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Tests;

public class CreateNodeHandlerTests
{
    private static readonly Mock<IValidator<CreateNodeCommand>> _mockValidator = new();
    private static readonly Mock<ITreeRepository> _mockTreeRepository = new();
    private static readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private static readonly Mock<ILogger<CreateNodeHandler>> _mockLogger = new();

    private readonly CreateNodeHandler _handler = new CreateNodeHandler(_mockValidator.Object,
        _mockTreeRepository.Object,
        _mockUnitOfWork.Object,
        _mockLogger.Object);


    [Fact]
    public async Task Execute_ShouldReturnNodeId_WhenCommandIsValid_AndParentNodeExists()
    {
        // Arrange
        var command = new CreateNodeCommand("Valid Title",
            "Valid Description",
            Guid.NewGuid());

        var parentNode = Node.Create(Guid.NewGuid(), Title.Create("Parent Title").Value,
            Description.Create("Parent Description").Value).Value;
        var newNodeId = Guid.NewGuid();

        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        _mockTreeRepository.Setup(repo => repo.GetById(command.ParentId!.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<Node, Error>(parentNode));
        _mockTreeRepository.Setup(repo => repo.Add(It.IsAny<Node>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Execute(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
        _mockTreeRepository.Verify(repo => repo.Add(It.IsAny<Node>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.SaveChanges(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Execute_ShouldReturnNodeId_WhenParentNodeDoesNotExist_AndNoParentId()
    {
        // Arrange
        var command = new CreateNodeCommand("Valid Title",
            "Valid Description",
            Guid.NewGuid());

        var parentNode = Node.Create(Guid.NewGuid(), Title.Create("Parent Title").Value,
            Description.Create("Parent Description").Value).Value;
        
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        _mockTreeRepository.Setup(repo => repo.GetById(command.ParentId!.Value, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<Node, Error>(parentNode));
        _mockTreeRepository.Setup(repo => repo.Add(It.IsAny<Node>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _mockUnitOfWork.Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Execute(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();
    }

    [Fact]
    public async Task Execute_ShouldReturnError_WhenValidationFails()
    {
        // Arrange
        var command = new CreateNodeCommand("", "Valid Description", null);

        var validationFailures = new List<ValidationFailure>
        {
            new(nameof(command.Title), Errors.Node.ValueIsRequired("title").Serialize()),
        };
        var validationResult = new ValidationResult(validationFailures);
        _mockValidator.Setup(v => v.ValidateAsync(command, It.IsAny<CancellationToken>()))
            .ReturnsAsync(validationResult);

        // Act
        var result = await _handler.Execute(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().ContainSingle(error => error.Message == $"The title is required.");
    }
}
