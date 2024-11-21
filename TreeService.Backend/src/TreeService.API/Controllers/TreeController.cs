using Microsoft.AspNetCore.Mvc;
using TreeService.API.Controllers.Requests;
using TreeService.API.Extensions;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Features.Commands.DeleteNode;
using TreeService.Application.Features.Commands.UpdateNodeInformation;
using TreeService.Application.Features.Queries.GetAllNodesById;
using TreeService.Application.Features.Queries.GetNodeById;


namespace TreeService.API.Controllers;

public class TreeController : ApplicationController
{
    [HttpPost]
    public async Task<IActionResult> CreateNode(
        [FromBody] CreateNodeRequest request,
        [FromServices] CreateNodeHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request.ToCommand(), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{nodeId:guid}")]
    public async Task<IActionResult> DeleteNode(
        [FromRoute] Guid nodeId,
        [FromServices] DeleteNodeHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(new DeleteNodeCommand(nodeId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpPatch("{nodeId:guid}")]
    public async Task<IActionResult> UpdateNode(
        [FromRoute] Guid nodeId,
        [FromBody] UpdateNodeInformationRequest request,
        [FromServices] UpdateNodeInformationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(request.ToCommand(nodeId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("{nodeId:guid}")]
    public async Task<IActionResult> GetNodeById(
        [FromRoute] Guid nodeId,
        [FromServices] GetNodeByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(new GetNodeByIdQuery(nodeId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpGet("{nodeId:guid}/childrens")]
    public async Task<IActionResult> GetAllNodeById(
        [FromRoute] Guid nodeId,
        [FromServices] GetAllNodesByIdHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Execute(new GetAllNodesByIdQuery(nodeId), cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
