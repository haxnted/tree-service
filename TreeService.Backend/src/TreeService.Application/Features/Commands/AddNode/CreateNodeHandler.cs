using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using TreeService.Application.Abstractions;
using TreeService.Application.Extensions;
using TreeService.Application.Interfaces;
using TreeService.Domain;
using TreeService.Domain.Shared;
using TreeService.Domain.ValueObjects;

namespace TreeService.Application.Features.Commands.AddNode;

/// <summary>
/// Добавление ноды
/// </summary>
public class CreateNodeHandler(
    IValidator<CreateNodeCommand> validator,
    ITreeRepository treeRepository,
    IUnitOfWork unitOfWork,
    ILogger<CreateNodeHandler> logger) : ICommandHandler<Guid, CreateNodeCommand>
{
    public async Task<Result<Guid, ErrorList>> Execute(
        CreateNodeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();

        Node tempNode;
        var title = Title.Create(command.Title).Value;
        var description = Description.Create(command.Description).Value;
        var createdAt = DateTime.UtcNow;

        // объясняю принцип работы:
        // Сначала проверяем на наличие родительского узла
        if (command.ParentId.HasValue)
        {
            var newNodeId = Guid.NewGuid();
            var node = await treeRepository.GetById(command.ParentId.Value, cancellationToken);
            // Если родительский узел найден, то добавляем к нему
            if (node.IsSuccess)
            {
                tempNode = Node.Create(newNodeId, title, description, node.Value.Id, node.Value).Value;
                await treeRepository.Add(tempNode, cancellationToken);
                
                treeRepository.Save(node.Value);
                await unitOfWork.SaveChanges(cancellationToken);
                return tempNode.Id;
            }
        }

        // Если родительский узел не указан, то создаем новый
        tempNode = Node.Create(Guid.NewGuid(), title, description).Value;
        await treeRepository.Add(tempNode, cancellationToken);
        await unitOfWork.SaveChanges(cancellationToken);

        logger.Log(LogLevel.Information, "Node {NodeId} created successfully", tempNode.Id);
        return tempNode.Id;
    }
}
