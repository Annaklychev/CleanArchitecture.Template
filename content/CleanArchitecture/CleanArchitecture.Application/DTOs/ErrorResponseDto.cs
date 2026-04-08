namespace CleanArchitecture.Application.DTOs;

/// <summary>
/// Error response DTO for API responses.
/// Contains localization key and parameters for client-side translation.
/// </summary>
public sealed record ErrorResponseDto(
    string Code,
    object? Parameters = null,
    string? TraceId = null);
