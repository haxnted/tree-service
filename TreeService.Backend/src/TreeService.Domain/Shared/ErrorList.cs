using System.Collections;
using System.Collections.Generic;

namespace TreeService.Domain.Shared;

/// <summary>
/// Коллекция ошибок
/// </summary>
public class ErrorList : IEnumerable<Error>
{
    private readonly List<Error> _errors;

    /// <summary>
    /// Создает экземпляр <see cref="ErrorList"/> из списка ошибок
    /// </summary>
    /// <param name="errors">Список ошибок</param>
    public ErrorList(IEnumerable<Error> errors)
    {
        _errors = new List<Error>(errors);
    }

    /// <summary>
    /// Возвращает перечислитель, который выполняет итерацию по коллекции ошибок
    /// </summary>
    /// <returns>Перечислитель</returns>
    public IEnumerator<Error> GetEnumerator()
        => _errors.GetEnumerator();

    /// <summary>
    /// Возвращает перечислитель, который выполняет итерацию по коллекции ошибок
    /// </summary>
    /// <returns>Перечислитель</returns>
    IEnumerator IEnumerable.GetEnumerator()
        => _errors.GetEnumerator();

    /// <summary>
    /// Неявное преобразование <see cref="List{Error}"/> к <see cref="ErrorList"/>
    /// </summary>
    /// <param name="errors">Список ошибок</param>
    public static implicit operator ErrorList(List<Error> errors)
        => new(errors);

    /// <summary>
    /// Неявное преобразование <see cref="Error"/> к <see cref="ErrorList"/>
    /// </summary>
    /// <param name="error">Ошибки</param>
    public static implicit operator ErrorList(Error error)
        => new(new[] { error });
}