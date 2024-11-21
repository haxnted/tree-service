using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TreeService.Application.Abstractions;
using TreeService.Application.Interfaces;
using TreeService.Infrastructure.DbContexts;
using TreeService.Infrastructure.Repositories;

namespace TreeService.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        return services
            .AddScoped<IMigrator, TreeMigrator>()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            .AddRepositories()
            .AddDbContexts(configuration);
    }

    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Database");
        services.AddScoped<TreeWriteDbContext>(provider => new TreeWriteDbContext(connectionString!));
        services.AddScoped<ITreeReadDbContext, TreeReadDbContext>(provider => new TreeReadDbContext(connectionString!));
        return services;
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        return services.AddScoped<ITreeRepository, TreeRepository>();
    }
}
