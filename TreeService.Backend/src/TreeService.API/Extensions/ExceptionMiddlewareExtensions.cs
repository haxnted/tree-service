using TreeService.API.Middlewares;

namespace TreeService.API.Extensions;

/// <summary>
///     Метод расширения для добавления middleware-а обработки исключений
/// </summary>
public static class ExceptionMiddlewareExtensions
{
    /// <summary>
    ///     Добавляет middleware-а обработки исключений
    /// </summary>
    /// <param name="builder">Билдер приложения</param>
    /// <returns>Билдер приложения</returns>
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        => builder.UseMiddleware<ExceptionMiddleware>();
}