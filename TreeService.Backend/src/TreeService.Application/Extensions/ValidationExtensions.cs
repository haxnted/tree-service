using FluentValidation.Results;
using TreeService.Domain.Shared;

namespace TreeService.Application.Extensions;

public static class ValidationExtensions
{
    /// <summary>
    /// Преобразует результат валидации в список ошибок
    /// </summary>
    /// <param name="validationResult">Результат валидации</param>
    /// <returns>Список ошибок</returns>
    public static ErrorList ToList(this ValidationResult validationResult)
    {
        var validationErrors = validationResult.Errors;

        var errors = from validationError in validationErrors
            let errorMessage = validationError.ErrorMessage
            let error = Error.Deserialize(errorMessage)
            select Error.Validation(error.Code, error.Message, validationError.PropertyName);

        return errors.ToList();
    }
}