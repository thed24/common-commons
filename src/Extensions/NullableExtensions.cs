using System.Runtime.CompilerServices;

namespace CommonCommons.Extensions;

public static class NullableExtensions
{
    /// <summary>
    /// Executes the mapper function <paramref name="map" /> on the given <paramref name="value" /> if it is not null and returns the result.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the <paramref name="map" />. </param>
    /// <param name="map"> The mapper function to be executed on the <paramref name="value" />. </param>
    /// <returns> The result of the mapper function <paramref name="map" /> if the <paramref name="value" /> is not null, otherwise null. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOutput? Map<TInput, TOutput>(this TInput? value, Func<TInput, TOutput> map)
    {
        return value is null
            ? default
            : map(value);
    }

    /// <summary>
    /// Executes the mapper function <paramref name="map" /> on the given <paramref name="value" /> if it is not null and returns the result.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the <paramref name="map" />. </param>
    /// <param name="map"> The mapper function to be executed on the <paramref name="value" />. </param>
    /// <returns> The result of the mapper function <paramref name="map" /> if the <paramref name="value" /> is not null, otherwise null. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput?> MapAsync<TInput, TOutput>(this TInput? value, Func<TInput, Task<TOutput>> map)
    {
        return value is null
            ? default
            : await map(value);
    }
    
    /// <summary>
    /// Executes the appropriate function depending on whether the given <paramref name="value" /> is null or not.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the appropriate function. </param>
    /// <param name="success"> The function to be executed on the <paramref name="value" /> if it is not null. </param>
    /// <param name="failure"> The function to be executed if the <paramref name="value" /> is null. </param>
    /// <returns> The result of the appropriate function. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOutput Match<TInput, TOutput>(this TInput? value, Func<TInput, TOutput> success, Func<TOutput> failure)
        where TInput : class
    {
        return value is null
            ? failure()
            : success(value);
    }
    
    /// <summary>
    /// Executes the appropriate function depending on whether the given <paramref name="value" /> is null or not.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the appropriate function. </param>
    /// <param name="success"> The function to be executed on the <paramref name="value" /> if it is not null. </param>
    /// <param name="failure"> The function to be executed if the <paramref name="value" /> is null. </param>
    /// <returns> The result of the appropriate function. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TOutput Match<TInput, TOutput>(this TInput? value, Func<TInput, TOutput> success, Func<TOutput> failure)
        where TInput : struct
    {
        return value.HasValue
            ? success(value.Value)
            : failure();
    }
    
    /// <summary>
    /// Executes the appropriate function depending on whether the given <paramref name="value" /> is null or not.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the appropriate function. </param>
    /// <param name="success"> The function to be executed on the <paramref name="value" /> if it is not null. </param>
    /// <param name="failure"> The function to be executed if the <paramref name="value" /> is null. </param>
    /// <returns> The result of the appropriate function. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput> MatchAsync<TInput, TOutput>(this TInput? value, Func<TInput, Task<TOutput>> success, Func<Task<TOutput>> failure)
        where TInput : class
    {
        return value is null
            ? await failure().ConfigureAwait(false)
            : await success(value).ConfigureAwait(false);
    }

    /// <summary>
    /// Executes the appropriate function depending on whether the given <paramref name="value" /> is null or not.
    /// </summary>
    /// <typeparam name="TInput"> The type of the value. </typeparam>
    /// <typeparam name="TOutput"> The type of the result. </typeparam>
    /// <param name="value"> The value to be passed to the appropriate function. </param>
    /// <param name="success"> The function to be executed on the <paramref name="value" /> if it is not null. </param>
    /// <param name="failure"> The function to be executed if the <paramref name="value" /> is null. </param>
    /// <returns> The result of the appropriate function. </returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static async Task<TOutput> MatchAsync<TInput, TOutput>(
        this TInput? value, 
        Func<TInput, Task<TOutput>> success, 
        Func<Task<TOutput>> failure)
        where TInput : struct
    {
        return value.HasValue
            ? await success(value.Value).ConfigureAwait(false)
            : await failure().ConfigureAwait(false);
    }
    
    /// <summary>
    /// Attempts to parse the given <paramref name="input" /> to an int and returns the result if successful, otherwise null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int? TryParseToInt(this string input)
    {
        return int.TryParse(input, out int result) ? result : null;
    }

    /// <summary>
    /// Attempts to parse the given <paramref name="input" /> to a double and returns the result if successful, otherwise null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double? TryParseToDouble(this string input)
    {
        return double.TryParse(input, out double result) ? result : null;
    }

    /// <summary>
    /// Attempts to parse the given <paramref name="input" /> to a decimal and returns the result if successful, otherwise null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static decimal? TryParseToDecimal(this string input)
    {
        return decimal.TryParse(input, out decimal result) ? result : null;
    }

    /// <summary>
    /// Attempts to parse the given <paramref name="input" /> to a DateTime and returns the result if successful, otherwise null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static DateTime? TryParseToDateTime(this string input)
    {
        return DateTime.TryParse(input, out DateTime result) ? result : null;
    }

    /// <summary>
    /// Attempts to parse the given <paramref name="input" /> to a bool and returns the result if successful, otherwise null.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool? TryParseToBool(this string input)
    {
        return bool.TryParse(input, out bool result) ? result : null;
    }
}