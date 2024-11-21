using TreeService.Application.Abstractions;

namespace TreeService.Application.Features.Commands.UpdateNodeInformation;

public record UpdateNodeInformationCommand(Guid NodeId, string Title, string Description) : ICommand;
