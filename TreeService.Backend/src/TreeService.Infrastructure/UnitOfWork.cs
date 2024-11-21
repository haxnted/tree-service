using System.Data.Common;
using Microsoft.EntityFrameworkCore.Storage;
using TreeService.Application.Interfaces;
using TreeService.Infrastructure.DbContexts;

namespace TreeService.Infrastructure;

/// <summary>
/// Класс UnitOfWork предоставляет методы для управления транзакциями и сохранения изменений в базе данных.
/// </summary>
internal class UnitOfWork : IUnitOfWork
{
    private readonly TreeWriteDbContext _writeDbContext;

    /// <summary>
    /// Конструктор, принимающий контекст базы данных для записи.
    /// </summary>
    /// <param name="writeDbContext">Контекст базы данных для записи.</param>
    public UnitOfWork(TreeWriteDbContext writeDbContext)
    {
        _writeDbContext = writeDbContext;
    }

    /// <summary>
    /// Начинает транзакцию в базе данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    /// <returns>Текущая транзакция базы данных.</returns>
    public async Task<DbTransaction> BeginTransaction(CancellationToken cancellationToken = default)
    {
        var transaction = await _writeDbContext.Database.BeginTransactionAsync(cancellationToken);
        return transaction.GetDbTransaction();
    }

    /// <summary>
    /// Сохраняет изменения в базе данных.
    /// </summary>
    /// <param name="cancellationToken">Токен отмены для асинхронной операции.</param>
    public async Task SaveChanges(CancellationToken cancellationToken = default)
    {
        await _writeDbContext.SaveChangesAsync(cancellationToken);
    }
}