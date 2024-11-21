using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using TreeService.Application.Abstractions;

namespace TreeService.Application;

/// <summary>
///     Методы расширения для регистрации Application Layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    ///     Регистрация Application Layer
    /// </summary>
    /// <param name="services">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        return services.AddHandlers()
            .AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
    }

    /// <summary>
    ///     Регистрация сервисов handlers с помощью библиотеки Scrutor
    /// </summary>
    /// <param name="collection">Коллекция сервисов</param>
    /// <returns>Коллекция сервисов</returns>
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