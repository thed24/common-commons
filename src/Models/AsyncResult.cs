using System.Runtime.CompilerServices;

namespace CommonCommons.Models;

/// <summary>
/// Represents an asynchronous operation that returns a result which can be either a success or a failure.
/// </summary>
/// <typeparam name="TValue">The type of the value in case of success.</typeparam>
/// <typeparam name="TError">The type of the error in case of failure.</typeparam>
public class AsyncResult<TValue, TError>
{
    private readonly Task<Result<TValue, TError>> _task;

    /// <summary>
    /// Initializes a new instance of the AsyncResult class with a task.
    /// </summary>
    /// <param name="task">The task that produces the result.</param>
    public AsyncResult(Task<Result<TValue, TError>> task)
    {
        _task = task;
    }

    /// <summary>
    /// Transforms the success value of this AsyncResult using an asynchronous function.
    /// </summary>
    /// <param name="map">The asynchronous function to transform the success value.</param>
    /// <typeparam name="TNewValue">The type of the value returned by the map function.</typeparam>
    /// <returns>A new AsyncResult with the transformed value.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public AsyncResult<TNewValue, TError> MapAsync<TNewValue>(Func<TValue, Task<TNewValue>> map)
    {
        var newTask = _task.ContinueWith(async t =>
        {
            if (t.Result.IsSuccess)
                return Result<TNewValue, TError>.Success(await map(t.Result.Value));
            else
                return Result<TNewValue, TError>.Failure(t.Result.Error);
        }).Unwrap();

        return new AsyncResult<TNewValue, TError>(newTask);
    }

    /// <summary>
    /// Processes the result asynchronously using one of two provided functions based on whether the result is a success or a failure.
    /// </summary>
    /// <param name="success">The function to process the success value.</param>
    /// <param name="failure">The function to process the failure value.</param>
    /// <typeparam name="TOutput">The type of the value returned by the success or failure function.</typeparam>
    /// <returns>A task representing the asynchronous operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<TOutput> MatchAsync<TOutput>(
        Func<TValue, Task<TOutput>> success, 
        Func<TError, Task<TOutput>> failure)
    {
        return _task.ContinueWith(async t =>
        {
            if (t.Result.IsSuccess)
                return await success(t.Result.Value);
            else
                return await failure(t.Result.Error);
        }).Unwrap();
    }

    /// <summary>
    /// Returns the underlying task of this AsyncResult.
    /// </summary>
    /// <returns>The task representing the asynchronous operation.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Task<Result<TValue, TError>> AsTask()
    {
        return _task;
    }
}
