using TreeService.Application.Abstractions;

namespace TreeService.Application.Features.Queries.GetNodeById;

public record GetNodeByIdQuery(Guid NodeId) : IQuery;
