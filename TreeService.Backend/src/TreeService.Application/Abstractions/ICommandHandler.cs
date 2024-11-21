using CSharpFunctionalExtensions;
using TreeService.Domain.Shared;

namespace TreeService.Application.Abstractions;

/// <summary>
///     Интерфейс для обработки команд
/// </summary>
/// <typeparam name="TResponse">Тип результата</typeparam>
/// <typeparam name="TCommand">Тип команды</typeparam>
public interface ICommandHandler<TResponse, in TCommand> where TCommand : ICommand
{
    /// <summary>
    ///     Выполняет команду
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Результат выполнения</returns>
    public Task<Result<TResponse, ErrorList>> Execute(TCommand command, CancellationToken cancellationToken = default);
}

/// <summary>
///     Интерфейс для обработки команд
/// </summary>
/// <typeparam name="TCommand">Тип команды</typeparam>
public interface ICommandHandler<in TCommand> where TCommand : ICommand
{
    /// <summary>
    ///     Выполняет команду
    /// </summary>
    /// <param name="command">Команда</param>
    /// <param name="token">Токен отмены</param>
    /// <returns>Результат выполнения</returns>
    public Task<UnitResult<ErrorList>> Execute(TCommand command, CancellationToken token = default);
}
