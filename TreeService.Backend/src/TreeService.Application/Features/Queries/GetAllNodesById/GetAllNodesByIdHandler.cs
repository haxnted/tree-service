using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TreeService.Application.Abstractions;
using TreeService.Application.Interfaces;
using TreeService.Contracts;
using TreeService.Domain.Shared;

namespace TreeService.Application.Features.Queries.GetAllNodesById;

/// <summary>
/// Возвращает все ноды по Id
/// </summary>
/// <param name="readDbContext">Контекст чтения</param>
public class GetAllNodesByIdHandler(
    ITreeReadDbContext readDbContext): IQueryHandler<NodeDto, GetAllNodesByIdQuery>
{
    public async Task<Result<NodeDto, ErrorList>> Execute(
        GetAllNodesByIdQuery query, CancellationToken cancellationToken = default)
    {
        var node = await readDbContext.Nodes
            .Include(n => n.ChildrenList)
            .FirstOrDefaultAsync(n => n.Id == query.NodeId, cancellationToken);
        if (node is null)
            return Errors.Node.NotFound(query.NodeId).ToErrorList();

        return node;
    }
}

public record GetAllNodesByIdQuery(Guid NodeId) : IQuery;
