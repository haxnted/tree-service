using TreeService.Application.Features.Commands.UpdateNodeInformation;

namespace TreeService.API.Controllers.Requests;

public record UpdateNodeInformationRequest( string Title, string Description)
{
    public UpdateNodeInformationCommand ToCommand(Guid nodeId) => new(nodeId, Title, Description);
}
