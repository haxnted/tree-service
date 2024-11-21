using TreeService.Application.Abstractions;

namespace TreeService.Application.Features.Commands.DeleteNode;

public record DeleteNodeCommand(Guid NodeId) : ICommand;
