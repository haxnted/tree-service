namespace TreeService.Application.Abstractions;

/// <summary>
/// Интерфейс для миграции бд
/// </summary>
public interface IMigrator
{
    Task Migrate(CancellationToken cancellationToken = default);
}