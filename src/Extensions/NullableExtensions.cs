using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace CommonCommons.Extensions;

public static class NullableExtensions
{
    [return: NotNullIfNotNull("value")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOutput? Map<TInput, TOutput>(this TInput? value, Func<TInput, TOutput> map)
    {
        return value is null
          ? default
          : map(value);
    }

    [return: NotNullIfNotNull("value")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput?> MapAsync<TInput, TOutput>(this TInput value, Func<TInput, Task<TOutput>> map)
    {
        return value is null
            ? default
            : await map(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOutput Match<TInput, TOutput>(this TInput? value, Func<TInput?, TOutput> success, Func<TInput?, TOutput> failure)
    {
        return value is null
          ? failure(value)
          : success(value);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput> MatchAsync<TInput, TOutput>(this TInput? value, Func<TInput, Task<TOutput>> success, Func<TInput?, Task<TOutput>> failure)
    {
        return value is null
          ? await failure(value)
          : await success(value);
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? TryParseToInt(this string input) => int.TryParse(input, out int result) ? result : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? TryParseToDouble(this string input) => double.TryParse(input, out double result) ? result : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? TryParseToDecimal(this string input) => decimal.TryParse(input, out decimal result) ? result : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? TryParseToDateTime(this string input) => DateTime.TryParse(input, out DateTime result) ? result : null;
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool? TryParseToBool(this string input) => bool.TryParse(input, out bool result) ? result : null;
}
