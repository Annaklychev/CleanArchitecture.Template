namespace CleanArchitecture.Application.Common;

/// <summary>
/// Application-level Result pattern for use cases.
/// Wraps success values or errors with localization keys.
/// </summary>
public class AppResult<T>
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public T? Value { get; }
    public AppError? Error { get; }

    private AppResult(T value)
    {
        IsSuccess = true;
        Value = value;
        Error = null;
    }

    private AppResult(AppError error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    public static AppResult<T> Success(T value) => new(value);
    public static AppResult<T> Fail(AppError error) => new(error);
    public static AppResult<T> Fail(string errorKey, object? parameters = null)
        => new(new AppError(errorKey, parameters));

    public TResult Match<TResult>(
        Func<T, TResult> onSuccess,
        Func<AppError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess(Value!) : onFailure(Error!);
    }
}

/// <summary>
/// Non-generic version for operations that don't return a value.
/// </summary>
public class AppResult
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public AppError? Error { get; }

    private AppResult(bool isSuccess, AppError? error = null)
    {
        IsSuccess = isSuccess;
        Error = error;
    }

    public static AppResult Success() => new(true);
    public static AppResult Fail(AppError error) => new(false, error);
    public static AppResult Fail(string errorKey, object? parameters = null)
        => new(false, new AppError(errorKey, parameters));

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<AppError, TResult> onFailure)
    {
        return IsSuccess ? onSuccess() : onFailure(Error!);
    }
}
