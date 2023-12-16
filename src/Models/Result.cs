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
public readonly record struct Result<TValue, TError>
{
    /// <summary>
    /// The value if it is in the success state.
    /// </summary>
    public TValue? Value { get; }
    /// <summary>
    /// The error if it is in the failure state.
    /// </summary>
    public TError? Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(TError error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(TValue value)
    {
        Value = value;
        Error = default;
        IsSuccess = true;
    }

    /// <summary>
    /// Create a new result in the success state.
    /// </summary>
    /// <param name="value"> The value to store in the result. </param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Success(TValue value)
    {
        return new Result<TValue, TError>(value);
    }

    /// <summary>
    /// Create a new result in the failure state.
    /// </summary>
    /// <param name="error"> The error to store in the result. </param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError>(error);
    }

    /// <summary>
    /// Map the value of the result to a new value.
    /// </summary>
    /// <param name="map"> The function to map the value. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the map function will be called with the value.
    /// If the result is in the failure state, the map function will not be called.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TOutput, TError> Map<TOutput>(Func<TValue, TOutput> map)
    {
        return IsSuccess
            ? Result<TOutput, TError>.Success(map(Value))
            : Result<TOutput, TError>.Failure(Error);
    }

    /// <summary>
    /// Map the value of the result to a new value asynchronously.
    /// </summary>
    /// <param name="map"> The function to map the value. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the map function will be called with the value.
    /// If the result is in the failure state, the map function will not be called.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AsyncResult<TNewValue, TError> MapAsync<TNewValue>(Func<TValue, Task<TNewValue>> map)
    {
        var task = IsSuccess
            ? map(Value).ContinueWith(t => Result<TNewValue, TError>.Success(t.Result))
            : Task.FromResult(Result<TNewValue, TError>.Failure(Error));

        return new AsyncResult<TNewValue, TError>(task);
    }

    /// <summary>
    /// Match the value of the result to a new value.
    /// </summary>
    /// <param name="success"> The function to map the value if the result is in the success state. </param>
    /// <param name="failure"> The function to map the error if the result is in the failure state. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the success function will be called with the value.
    /// If the result is in the failure state, the failure function will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<TError, TOutput> failure)
    {
        return IsSuccess
            ? success(Value)
            : failure(Error);
    }

    /// <summary>
    /// Match the value of the result to a new value asynchronously.
    /// </summary>
    /// <param name="success"> The function to map the value if the result is in the success state. </param>
    /// <param name="failure"> The function to map the error if the result is in the failure state. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the success function will be called with the value.
    /// If the result is in the failure state, the failure function will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<TOutput> MatchAsync<TOutput>(Func<TValue, Task<TOutput>> success, Func<TError, Task<TOutput>> failure)
    {
        return IsSuccess
            ? await success(Value).ConfigureAwait(false)
            : await failure(Error).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Execute an action based on the state of the result.
    /// </summary>
    /// <param name="success"> The action to execute if the result is in the success state. </param>
    /// <param name="failure"> The action to execute if the result is in the failure state. </param>
    /// <remarks>
    /// If the result is in the success state, the success action will be called with the value.
    /// If the result is in the failure state, the failure action will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TValue, TError> Do(Action<TValue> success, Action<TError> failure)
    {
        if (IsSuccess)
        {
            success(Value);
        }
        else
        {
            failure(Error);
        }

        return this;
    }
    
    /// <summary>
    /// Execute an action based on the state of the result asynchronously.
    /// </summary>
    /// <param name="success"> The action to execute if the result is in the success state. </param>
    /// <param name="failure"> The action to execute if the result is in the failure state. </param>
    /// <remarks>
    /// If the result is in the success state, the success action will be called with the value.
    /// If the result is in the failure state, the failure action will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<Result<TValue, TError>> DoAsync(Func<TValue, Task> success, Func<TError, Task> failure)
    {
        if (IsSuccess)
        {
            await success(Value).ConfigureAwait(false);
        }
        else
        {
            await failure(Error).ConfigureAwait(false);
        }

        return this;
    }
}

/// <summary>
/// A simpler result type that can be in either a success or failure state.
/// </summary>
/// <typeparam name="TValue"> The type of the value if it is in the success state. </typeparam>
public readonly record struct Result<TValue>
{
    /// <summary>
    /// The value if it is in the success state.
    /// </summary>
    public TValue? Value { get; }
    /// <summary>
    /// The error if it is in the failure state.
    /// </summary>
    public string? Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(string error)
    {
        Value = default;
        Error = error;
        IsSuccess = false;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private Result(TValue value)
    {
        Value = value;
        Error = null;
        IsSuccess = true;
    }

    /// <summary>
    /// Create a new result in the success state.
    /// </summary>
    /// <param name="value"> The value to store in the result. </param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Success(TValue value)
    {
        return new Result<TValue>(value);
    }

    /// <summary>
    /// Create a new result in the failure state.
    /// </summary>
    /// <param name="error"> The error to store in the result. </param>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Result<TValue> Failure(string error)
    {
        return new Result<TValue>(error);
    }

    /// <summary>
    /// Map the value of the result to a new value.
    /// </summary>
    /// <param name="map"> The function to map the value. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the map function will be called with the value.
    /// If the result is in the failure state, the map function will not be called.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TOutput> Map<TOutput>(Func<TValue, TOutput> map)
    {
        return IsSuccess
            ? Result<TOutput>.Success(map(Value))
            : Result<TOutput>.Failure(Error);
    }

    /// <summary>
    /// Map the value of the result to a new value asynchronously.
    /// </summary>
    /// <param name="map"> The function to map the value. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the map function will be called with the value.
    /// If the result is in the failure state, the map function will not be called.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<Result<TValue2>> MapAsync<TValue2>(Func<TValue, Task<TValue2>> map)
    {
        return IsSuccess
            ? Result<TValue2>.Success(await map(Value))
            : Result<TValue2>.Failure(Error);
    }

    /// <summary>
    /// Match the value of the result to a new value.
    /// </summary>
    /// <param name="success"> The function to map the value if the result is in the success state. </param>
    /// <param name="failure"> The function to map the error if the result is in the failure state. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the success function will be called with the value.
    /// If the result is in the failure state, the failure function will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<string, TOutput> failure)
    {
        return IsSuccess
            ? success(Value)
            : failure(Error);
    }

    /// <summary>
    /// Match the value of the result to a new value asynchronously.
    /// </summary>
    /// <param name="success"> The function to map the value if the result is in the success state. </param>
    /// <param name="failure"> The function to map the error if the result is in the failure state. </param>
    /// <typeparam name="TOutput"> The type of the new value. </typeparam>
    /// <returns> A new result with the mapped value. </returns>
    /// <remarks>
    /// If the result is in the success state, the success function will be called with the value.
    /// If the result is in the failure state, the failure function will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<TOutput> MatchAsync<TOutput>(Func<TValue, Task<TOutput>> success, Func<string, Task<TOutput>> failure)
    {
        return IsSuccess
            ? await success(Value).ConfigureAwait(false)
            : await failure(Error).ConfigureAwait(false);
    }
    
    /// <summary>
    /// Execute an action based on the state of the result.
    /// </summary>
    /// <param name="success"> The action to execute if the result is in the success state. </param>
    /// <param name="failure"> The action to execute if the result is in the failure state. </param>
    /// <remarks>
    /// If the result is in the success state, the success action will be called with the value.
    /// If the result is in the failure state, the failure action will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Result<TValue> Do(Action<TValue> success, Action<string> failure)
    {
        if (IsSuccess)
        {
            success(Value);
        }
        else
        {
            failure(Error);
        }

        return this;
    }
    
    /// <summary>
    /// Execute an action based on the state of the result asynchronously.
    /// </summary>
    /// <param name="success"> The action to execute if the result is in the success state. </param>
    /// <param name="failure"> The action to execute if the result is in the failure state. </param>
    /// <remarks>
    /// If the result is in the success state, the success action will be called with the value.
    /// If the result is in the failure state, the failure action will be called with the error.
    /// </remarks>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public async Task<Result<TValue>> DoAsync(Func<TValue, Task> success, Func<string, Task> failure)
    {
        if (IsSuccess)
        {
            await success(Value).ConfigureAwait(false);
        }
        else
        {
            await failure(Error).ConfigureAwait(false);
        }

        return this;
    }
}
