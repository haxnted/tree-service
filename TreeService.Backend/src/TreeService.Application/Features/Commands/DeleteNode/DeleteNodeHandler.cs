using CSharpFunctionalExtensions;
using Microsoft.Extensions.Logging;
using TreeService.Application.Abstractions;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Interfaces;
using TreeService.Domain.Shared;

namespace TreeService.Application.Features.Commands.DeleteNode;

/// <summary>
/// Удаляет ноде
/// </summary>
public class DeleteNodeHandler(
    ITreeRepository treeRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateNodeHandler> logger) : ICommandHandler<DeleteNodeCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(DeleteNodeCommand command, CancellationToken token = default)
    {
        var node = await treeRepository.GetById(command.NodeId, token);
        if (node.IsFailure)
            return node.Error.ToErrorList();
        
        treeRepository.Delete(node.Value);
        await unitOfWork.SaveChanges(token);
        logger.Log(LogLevel.Information, "Node {nodeId} was deleted", node.Value.Id);
        return UnitResult.Success<ErrorList>();
        
    }
}