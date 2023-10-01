namespace CommonCommons.Extensions;

public static class GenericExtensions
{
    public static void Do<T>(this T value, Action<T> action)
    {
        action(value);
    }
}