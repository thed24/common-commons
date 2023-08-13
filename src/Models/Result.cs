using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommonCommons.Models;

/// <summary>
/// A result type that can be in either a success or failure state.
/// This can be used in scenarios that may fail, but you don't want to throw an exception.
/// </summary>
/// <typeparam name="TValue">
/// The type of the value if it is in the success state.
/// </typeparam>
/// <typeparam name="TError">
/// The type of the error if it is in the failure state.
/// </typeparam>
public class Result<TValue, TError>
{
    public TValue? Value { get; }
    public TError? Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(TError error)
    {
        Error = error;
        IsSuccess = false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(TValue value)
    {
        Value = value;
        IsSuccess = true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Success(TValue value)
    {
        return new Result<TValue, TError>(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError>(error);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TOutput, TError> Map<TOutput>(Func<TValue, TOutput> map)
    {
        return IsSuccess
            ? Result<TOutput, TError>.Success(map(Value))
            : Result<TOutput, TError>.Failure(Error);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<Result<TValue2, TError>> MapAsync<TValue2>(Func<TValue, Task<TValue2>> map)
    {
        return IsSuccess
            ? Result<TValue2, TError>.Success(await map(Value))
            : Result<TValue2, TError>.Failure(Error);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<TError, TOutput> failure)
    {
        return IsSuccess
            ? success(Value)
            : failure(Error);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<TOutput> MatchAsync<TOutput>(Func<TValue, Task<TOutput>> success, Func<TError, Task<TOutput>> failure)
    {
        return IsSuccess
            ? await success(Value)
            : await failure(Error);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Do(Action<TValue> success, Action<TError> failure)
    {
        if (IsSuccess)
        {
            success(Value);
        }
        else
        {
            failure(Error);
        }
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task DoAsync(Func<TValue, Task> success, Func<TError, Task> failure)
    {
        if (IsSuccess)
        {
            await success(Value);
        }
        else
        {
            await failure(Error);
        }
    }
}
