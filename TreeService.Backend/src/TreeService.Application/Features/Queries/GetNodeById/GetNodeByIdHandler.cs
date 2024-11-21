using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TreeService.Application.Abstractions;
using TreeService.Application.Interfaces;
using TreeService.Contracts;
using TreeService.Domain.Shared;

namespace TreeService.Application.Features.Queries.GetNodeById;


/// <summary>
/// Возвращает ноду по Id
/// </summary>
/// <param name="readDbContext">Контекст чтения</param>
public class GetNodeByIdHandler(ITreeReadDbContext readDbContext) : IQueryHandler<NodeDto, GetNodeByIdQuery>
{
    public async Task<Result<NodeDto, ErrorList>> Execute(
        GetNodeByIdQuery query, CancellationToken cancellationToken = default)
    {
        var node = await readDbContext.Nodes.FirstOrDefaultAsync(n => n.Id == query.NodeId,cancellationToken);
        if (node is null)
            return Errors.Node.NotFound(query.NodeId).ToErrorList();

        return node;
    }
}