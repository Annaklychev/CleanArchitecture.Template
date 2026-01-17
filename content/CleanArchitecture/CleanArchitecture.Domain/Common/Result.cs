namespace CleanArchitecture.Domain.Common;

/// <summary>
/// Represents the result of an operation that can succeed or fail.
/// Generic version returns a value on success.
/// </summary>
public class Result<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public ResultError? Error { get; }

    private Result(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    private Result(ResultError error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static Result<T> Success(T value) => new(value);
    public static Result<T> Fail(ResultError error) => new(error);
    public static Result<T> Fail(string errorKey, object? parameters = null)
        => new(new ResultError(errorKey, parameters));

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<ResultError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}

/// <summary>
/// Non-generic result for operations that don't return a value.
/// </summary>
public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ResultError? Error { get; }

    private Result(bool isSuccess, ResultError? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static Result Success() => new(true);
    public static Result Fail(ResultError error) => new(false, error);
    public static Result Fail(string errorKey, object? parameters = null)
        => new(false, new ResultError(errorKey, parameters));

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<ResultError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}

/// <summary>
/// Represents an error with a localization key and optional parameters.
/// </summary>
public sealed record ResultError(string Code, object? Parameters = null);
