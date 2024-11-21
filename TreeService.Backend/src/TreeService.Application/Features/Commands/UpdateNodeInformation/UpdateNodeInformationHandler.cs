using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TreeService.Application.Abstractions;
using TreeService.Application.Extensions;
using TreeService.Application.Features.Commands.AddNode;
using TreeService.Application.Interfaces;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Features.Commands.UpdateNodeInformation;

/// <summary>
/// Обновляет информацию о ноде
/// </summary>
public class UpdateNodeInformationHandler(
    IValidator<UpdateNodeInformationCommand> validator,
    ITreeRepository treeRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateNodeHandler> logger) : ICommandHandler<UpdateNodeInformationCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(UpdateNodeInformationCommand command, CancellationToken token = default)
    {
        var validateResult = await validator.ValidateAsync(command, token);
        if (validateResult.IsValid == false)
            return validateResult.ToList();
        
        var node = await treeRepository.GetById(command.NodeId, token);
        if (node.IsFailure)
            return node.Error.ToErrorList();
        
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        
        node.Value.UpdateInformation(title, description);
        treeRepository.Save(node.Value);
        await unitOfWork.SaveChanges(token);
        
        logger.Log(LogLevel.Information, "Node {nodeId} was updated", node.Value.Id);
        return UnitResult.Success<ErrorList>();
    }
}