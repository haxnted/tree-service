using TreeService.Application.Abstractions;

namespace TreeService.API.Extensions;

/// <summary>
/// Расширения для запуска миграций
/// </summary>
public static class MigratorExtensions
{
    /// <summary>
    /// Выполнить миграции на всех зарегистрированных миграторах
    /// </summary>
    public static async Task RunMigrations(this IServiceProvider serviceProvider)
    {
        await using var scope = serviceProvider.CreateAsyncScope();
        var migrators = scope.ServiceProvider.GetRequiredService<IEnumerable<IMigrator>>();
        foreach (var migrator in migrators)
        {
            await migrator.Migrate();  
        }
    }
}