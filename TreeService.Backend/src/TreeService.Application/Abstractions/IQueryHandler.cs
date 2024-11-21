using CSharpFunctionalExtensions;
using TreeService.Domain.Shared;

namespace TreeService.Application.Abstractions;

/// <summary>
///     Интерфейс для обработки запросов
/// </summary>
/// <typeparam name="TResponse"> Тип результата </typeparam>
/// <typeparam name="TQuery"> Тип запроса </typeparam>
public interface IQueryHandler<TResponse, in TQuery> where TQuery : IQuery
{
    /// <summary>
    ///     Выполняет запрос
    /// </summary>
    /// <param name="query"> Запрос </param>
    /// <param name="cancellationToken"> Токен отмены </param>
    /// <returns> Результат выполнения запроса </returns>
    Task<Result<TResponse, ErrorList>> Execute(TQuery query, CancellationToken cancellationToken = default);
}