using Microsoft.AspNetCore.Mvc;
using TreeService.API.Responses;
using TreeService.Domain.Shared;

namespace TreeService.API.Extensions;

/// <summary>
/// Расширения для преобразования ошибок в HTTP-ответы
/// </summary>
public static class ResponseExtensions
{
    /// <summary>
    /// Преобразует ошибку в HTTP-ответ с соответствующим кодом состояния
    /// </summary>
    /// <param name="error">Ошибка</param>
    /// <returns>HTTP-ответ</returns>
    public static ActionResult ToResponse(this Error error)
    {
        var statusCode = GetStatusCodeForErrorType(error.Type);

        var envelope = Envelope.Error(error.ToErrorList());

        return new ObjectResult(envelope)
        {
            StatusCode = statusCode
        };
    }

    /// <summary>
    /// Преобразует список ошибок в HTTP-ответ с соответствующим кодом состояния
    /// </summary>
    /// <param name="errors">Список ошибок</param>
    /// <returns>HTTP-ответ</returns>
    public static ActionResult ToResponse(this ErrorList errors)
    {
        if (errors.Any())
            return new ObjectResult(Envelope.Error(errors))
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

        var distinctErrorTypes = errors
            .Select(x => x.Type)
            .Distinct()
            .ToList();

        var statusCode = distinctErrorTypes.Count > 1
            ? StatusCodes.Status500InternalServerError
            : GetStatusCodeForErrorType(distinctErrorTypes.First());

        var envelope = Envelope.Error(errors);

        return new ObjectResult(envelope)
        {
            StatusCode = StatusCodes.Status400BadRequest
        };
    }

    /// <summary>
    /// Возвращает код состояния HTTP для данного типа ошибки
    /// </summary>
    /// <param name="errorType">Тип ошибки</param>
    /// <returns>Код состояния HTTP</returns>
    private static int GetStatusCodeForErrorType(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Failure => StatusCodes.Status500InternalServerError,
            _ => StatusCodes.Status500InternalServerError
        };
}