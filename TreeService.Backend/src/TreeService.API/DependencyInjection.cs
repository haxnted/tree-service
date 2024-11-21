using TreeService.Application;
using TreeService.Infrastructure;

namespace TreeService.API;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler
                    = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddApplication()
            .AddInfrastructure(configuration);

        return services;
    }
}
