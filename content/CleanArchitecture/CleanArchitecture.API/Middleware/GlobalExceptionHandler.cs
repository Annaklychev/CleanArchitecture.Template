using System.Net;
using CleanArchitecture.Application.DTOs;
using CleanArchitecture.Domain.Errors;
using Microsoft.AspNetCore.Diagnostics;

namespace CleanArchitecture.API.Middleware;

/// <summary>
/// Global exception handler that converts domain exceptions to structured error responses
/// with localization keys. Does not translate - just returns keys for client-side localization.
/// </summary>
public sealed class GlobalExceptionHandler : IExceptionHandler
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
        var (statusCode, errorResponse) = exception switch
        {
            DomainException domainEx => MapDomainException(domainEx, httpContext),
            _ => MapUnexpectedException(exception, httpContext)
        };

        httpContext.Response.StatusCode = (int)statusCode;
        await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

        return true;
    }

    private (HttpStatusCode, ErrorResponseDto) MapDomainException(
        DomainException exception,
        HttpContext context)
    {
        _logger.LogWarning(exception, "Domain exception occurred: {ErrorKey}", exception.ErrorKey);

        var statusCode = exception.ErrorKey switch
        {
            var key when key.Contains("not_found") => HttpStatusCode.NotFound,
            var key when key.Contains("unauthorized") || key.Contains("forbidden") => HttpStatusCode.Forbidden,
            var key when key.Contains("invalid") || key.Contains("validation") => HttpStatusCode.BadRequest,
            var key when key.Contains("already_exists") => HttpStatusCode.Conflict,
            _ => HttpStatusCode.BadRequest
        };

        var errorResponse = new ErrorResponseDto(
            exception.ErrorKey,
            exception.Parameters,
            context.TraceIdentifier);

        return (statusCode, errorResponse);
    }

    private (HttpStatusCode, ErrorResponseDto) MapUnexpectedException(
        Exception exception,
        HttpContext context)
    {
        _logger.LogError(exception, "Unexpected error occurred");

        var errorResponse = new ErrorResponseDto(
            DomainErrorKeys.Common.UnexpectedError,
            null,
            context.TraceIdentifier);

        return (HttpStatusCode.InternalServerError, errorResponse);
    }
}
