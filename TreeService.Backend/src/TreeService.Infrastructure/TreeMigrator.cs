using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TreeService.Application.Abstractions;
using TreeService.Infrastructure.DbContexts;

namespace TreeService.Infrastructure;

/// <summary>
///     Миграция БД
/// </summary>
public class TreeMigrator(TreeWriteDbContext context, ILogger<TreeMigrator> logger) : IMigrator
{
    /// <summary>
    ///     Выполнение миграции БД
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        if (await context.Database.CanConnectAsync(cancellationToken) == false)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
        logger.Log(LogLevel.Information, "Applying accounts migrations...");
        await context.Database.MigrateAsync(cancellationToken);
        logger.Log(LogLevel.Information, "Migrations accounts applied successfully.");
    }
}