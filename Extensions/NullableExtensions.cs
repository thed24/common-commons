using System.Diagnostics.CodeAnalysis;

namespace CommonCommons.Extensions;

public static class NullableExtensions
{
    [return: NotNullIfNotNull("value")]
    public static TOutput? Map<TInput, TOutput>(this Nullable<TInput> value, Func<TInput, TOutput> map)
        where TInput : struct
    {
        return !value.HasValue
          ? default
          : map(value.Value);
    }

    [return: NotNullIfNotNull("value")]
    public static async Task<TOutput?> MapAsync<TInput, TOutput>(this Nullable<TInput> value, Func<TInput, Task<TOutput>> map)
        where TInput : struct
    {
        return !value.HasValue
            ? default
            : await map(value.Value);
    }

    public static TOutput Match<TInput, TOutput>(this Nullable<TInput> value, Func<TInput, TOutput> success, Func<Nullable<TInput>, TOutput> failure)
        where TInput : struct
    {
        return !value.HasValue
          ? failure(value)
          : success(value.Value);
    }

    public static async Task<TOutput> MatchAsync<TInput, TOutput>(this Nullable<TInput> value, Func<TInput, Task<TOutput>> success, Func<Nullable<TInput>, Task<TOutput>> failure)
        where TInput : struct
    {
        return !value.HasValue
          ? await failure(value)
          : await success(value.Value);
    }
}

internal class Examples
{
    public void Main()
    {
        int multiplyByTwo(int number) => number * 2;

        int? maybeInt = null;
        int? stillMaybeInt = maybeInt.Map(multiplyByTwo);
        int actualInt = stillMaybeInt.Match(
            success: multiplyByTwo,
            failure: _ => 0
        );
    }

    public async Task MainAsync()
    {
        Task<int> multiplyByTwo(int number) => Task.FromResult(number * 2);

        int? maybeInt = null;
        int? stillMaybeInt = await maybeInt.MapAsync(multiplyByTwo);
        int actualInt = await stillMaybeInt.MatchAsync(
            success: multiplyByTwo,
            failure: _ => Task.FromResult(0)
        );
    }
}

