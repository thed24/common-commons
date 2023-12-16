namespace CommonCommons.Extensions;

public static class GenericExtensions
{
    /// <summary>
    /// Executes the given <paramref name="action" /> on the given <paramref name="value" /> and returns itself to be potentially chained.
    /// </summary>
    /// <typeparam name="T"> The type of the value. </typeparam>
    /// <param name="value"> The value to be passed to the <paramref name="action" />. </param>
    /// <param name="action"> The action to be executed on the <paramref name="value" />. </param>
    public static T Do<T>(this T value, Action<T> action)
    {
        action(value);

        return value;
    }
}