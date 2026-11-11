using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
namespace MoodFlow.API.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);

        // اگر خطا از جنس ولیدیشن بود، وضعیت را 400 بگذار و پیام‌ها را دسته‌بندی کن
        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    failureGroup => failureGroup.Key,
                    failureGroup => failureGroup.Select(f => f.ErrorMessage).ToArray()
                );

            var validationProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Validation Error",
                Detail = "یک یا چند خطای اعتبارسنجی رخ داده است.",
                Instance = httpContext.Request.Path
            };

            validationProblemDetails.Extensions.Add("errors", errors);

            await httpContext.Response.WriteAsJsonAsync(validationProblemDetails, cancellationToken);
            return true;
        }
        // اگر خطا مربوط به پیدا نشدن داده (404) بود
        if (exception is KeyNotFoundException keyNotFoundException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;

            var notFoundDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "Not Found",
                Detail = keyNotFoundException.Message,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(notFoundDetails, cancellationToken);
            return true;
        }

        // اگر خطا مربوط به عدم دسترسی/احراز هویت (401) بود
        // ✅ کد جدید (تغییر فقط در شرط if است)
        if (exception is UnauthorizedAccessException or SecurityTokenException)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;

            var unauthorizedDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Title = "Unauthorized",
                Detail = exception.Message,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(unauthorizedDetails, cancellationToken);
            return true;
        }        // برای سایر خطاهای پیش‌بینی نشده سرور (خطای واقعی 500)
        httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
        {
            Status = (int)HttpStatusCode.InternalServerError,
            Title = "Server Error",
            Detail = "یک خطای داخلی در سرور رخ داده است."
        }, cancellationToken);

        return true;
    }
}