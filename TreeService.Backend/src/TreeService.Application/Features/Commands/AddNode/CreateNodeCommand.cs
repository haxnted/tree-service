using TreeService.Application.Abstractions;

namespace TreeService.Application.Features.Commands.AddNode;

public record CreateNodeCommand(
    string Title,
    string Description,
    Guid? ParentId) : ICommand;
