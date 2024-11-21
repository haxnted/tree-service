using System.Net;
using TreeService.API.Responses;
using TreeService.Domain.Shared;

namespace TreeService.API.Middlewares;

/// <summary>
///     Обработчик исключений
/// </summary>
public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    ///     Метод вызова следующего обработчика
    /// </summary>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Возникло внутреннее исключение на сервере");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var error = Error.Failure("server.internal.error", "Возникло внутреннее исключение на сервере", null);
            var envelope = Envelope.Error(error);

            await context.Response.WriteAsJsonAsync(envelope);
        }
    }
}