using CSharpFunctionalExtensions;
using FluentValidation;
using TreeService.Domain.Shared;

namespace TreeService.Application.Validator;

public static class CustomValidators
{
    /// <summary>
    ///     Проверяет, что значение может быть успешно преобразовано в value object.
    /// </summary>
    /// <typeparam name="T">Тип родительского объекта.</typeparam>
    /// <typeparam name="TElement">Тип проверяемого свойства.</typeparam>
    /// <typeparam name="TValueObject">Тип объекта значения.</typeparam>
    /// <param name="ruleBuilder">Построитель правил, к которому добавляется правило валидации.</param>
    /// <param name="factoryMethod">Фабричный метод для создания объекта значения из значения свойства.</param>
    /// <returns>Построитель правил с добавленным правилом валидации.</returns>
    public static IRuleBuilderOptionsConditions<T, TElement> MustBeValueObject<T, TElement, TValueObject>(
        this IRuleBuilder<T, TElement> ruleBuilder,
        Func<TElement, Result<TValueObject, Error>> factoryMethod)
    {
        return ruleBuilder.Custom((value, context) =>
        {
            var result = factoryMethod(value);
            if (result.IsSuccess)
                return;

            context.AddFailure(result.Error.Serialize());
        });
    }
}