using TreeService.Application.Features.Commands.AddNode;

namespace TreeService.API.Controllers.Requests;

public record CreateNodeRequest(
    string Title,
    string Description,
    Guid? ParentId)
{
    public CreateNodeCommand ToCommand() => new(Title, Description, ParentId);
}
