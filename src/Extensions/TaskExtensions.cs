using System.Runtime.CompilerServices;
using CommonCommons.Models;

namespace CommonCommons.Extensions;

public static class TaskExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput> MatchAsync<TInput, TError, TOutput>(this Task<Result<TInput, TError>> task, Func<TInput, Task<TOutput>> success, Func<TError, Task<TOutput>> failure)
    {
        Result<TInput, TError> result = await task;

        return result.IsSuccess
            ? await success(result.Value)
            : await failure(result.Error);
    }
}