using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TreeService.Application.Abstractions;

namespace TreeService.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddHandlers()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }
    private static IServiceCollection AddHandlers(this IServiceCollection collection)
    {
        collection.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        collection.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
            .AddClasses(classes => classes
                .AssignableTo(typeof(IQueryHandler<,>)))
            .AsSelfWithInterfaces()
            .WithScopedLifetime());

        return collection;
    }
}
