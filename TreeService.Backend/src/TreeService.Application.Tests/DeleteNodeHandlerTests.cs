using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Features.Commands.DeleteNode;
using TreeService.Application.Interfaces;
using TreeService.Domain;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Tests;

public class DeleteNodeHandlerTests
{
    private static readonly Mock<ITreeRepository> _mockTreeRepository = new();
    private static readonly Mock<IUnitOfWork> _mockUnitOfWork = new();
    private static readonly Mock<ILogger<CreateNodeHandler>> _mockLogger = new();

    private readonly DeleteNodeHandler _handler = new DeleteNodeHandler(_mockTreeRepository.Object,
        _mockUnitOfWork.Object,
        _mockLogger.Object);


    /// <summary>
    /// Проверяет успешное удаление узла.
    /// </summary>
    [Fact(DisplayName = "DeleteNodeHandler deletes node successfully")]
    public async Task Execute_NodeExists_ShouldDeleteNode()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var existingNode = Node.Create(nodeId, Title.Create("title").Value, Description.Create("description").Value)
            .Value;
        _mockTreeRepository
            .Setup(repo => repo.GetById(nodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Success<Node, Error>(existingNode));

        _mockUnitOfWork
            .Setup(uow => uow.SaveChanges(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Execute(new DeleteNodeCommand(nodeId));

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    /// <summary>
    /// Проверяет поведение при отсутствии узла.
    /// </summary>
    [Fact(DisplayName = "DeleteNodeHandler returns error when node is not found")]
    public async Task Execute_NodeNotFound_ShouldReturnError()
    {
        // Arrange
        var nodeId = Guid.NewGuid();
        var error = Errors.Node.NotFound(nodeId);

        _mockTreeRepository
            .Setup(repo => repo.GetById(nodeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(Result.Failure<Node, Error>(error));

        // Act
        var result = await _handler.Execute(new DeleteNodeCommand(nodeId));

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().ContainSingle(e => e.Message == $"Node with ID '{nodeId}' was not found.");
    }
}
