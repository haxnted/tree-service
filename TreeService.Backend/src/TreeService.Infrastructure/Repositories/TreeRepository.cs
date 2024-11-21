using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TreeService.Application.Interfaces;
using TreeService.Domain;
using TreeService.Domain.Shared;
using TreeService.Infrastructure.DbContexts;

namespace TreeService.Infrastructure.Repositories;

public class TreeRepository : ITreeRepository
{
    private readonly TreeWriteDbContext _writeDbContext;

    public TreeRepository(TreeWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    public async Task Add(Node node, CancellationToken cancellationToken = default)
    {
        await _writeDbContext.Nodes.AddAsync(node, cancellationToken);
    }

    /// <summary>
    /// Прикрепляет изменения ноды
    /// </summary>
    /// <param name="node">Нода</param>
    public void Save(Node node)
    {
        _writeDbContext.Nodes.Attach(node);
    }

    /// <summary>
    /// Получение ноды
    /// </summary>
    /// <param name="nodeId">Номер ноды</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Возвращает ноду если такая есть, в противном случае ошибку "не найдено"</returns>
    public async Task<Result<Node, Error>> GetById(Guid nodeId, CancellationToken cancellationToken = default)
    {
        var node = await _writeDbContext.Nodes
            .Include(n => n.ChildrenList)
            .FirstOrDefaultAsync(d => d.Id == nodeId, cancellationToken: cancellationToken);

        if (node is null)
            return Errors.Node.NotFound(nodeId);

        return node;
    }

    /// <summary>
    /// Удаление ноды
    /// </summary>
    /// <param name="node">нода</param>
    public void Delete(Node node)
    {
        _writeDbContext.Nodes.Remove(node);
    }
}
