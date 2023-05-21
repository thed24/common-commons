using System.Diagnostics.CodeAnalysis;

namespace CommonCommons.Models;

public class Result<TValue, TError>
{
    public TValue? Value { get; set; }
    public TError? Error { get; set; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Value is not null;

    public static Result<TValue, TError> Success(TValue value)
    {
        return new Result<TValue, TError> { Value = value };
    }

    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError> { Error = error };
    }

    public Result<TValue2, TError> Map<TValue2>(Func<TValue, TValue2> map)
    {
        return IsSuccess
            ? Result<TValue2, TError>.Success(map(Value))
            : Result<TValue2, TError>.Failure(Error);
    }

    public async Task<Result<TValue2, TError>> MapAsync<TValue2>(Func<TValue, Task<TValue2>> map)
    {
        return IsSuccess
            ? Result<TValue2, TError>.Success(await map(Value))
            : Result<TValue2, TError>.Failure(Error);
    }

    public TOutput Match<TOutput>(Func<TValue, TOutput> success, Func<TError, TOutput> failure)
    {
        return IsSuccess
            ? success(Value)
            : failure(Error);
    }

    public async Task<TOutput> MatchAsync<TOutput>(Func<TValue, Task<TOutput>> success, Func<TError, Task<TOutput>> failure)
    {
        return IsSuccess
            ? await success(Value)
            : await failure(Error);
    }
}

internal class Examples
{
    private record SomeErrorType(string Error, int Code);

    private Result<int, SomeErrorType> SomeMethodThatReturnsResult()
    {
        SomeErrorType error = new("We haven't implemented this!", -1);
        return Result<int, SomeErrorType>.Failure(error);
    }

    public void Main()
    {
        Result<int, SomeErrorType> result = SomeMethodThatReturnsResult();

        if (result.IsSuccess)
        {
            int value = result.Value;
            Console.WriteLine($"woo, we got ${value}!");
        }
        else
        {
            SomeErrorType error = result.Error;
            Console.WriteLine($"boo, we got error number ${error.Code}!");
        }

        string messageMatch = result.Match(
            success: value => $"woo, we got ${value}!",
            failure: error => $"boo, we got error number ${error.Code}!"
        );

        Console.WriteLine(messageMatch);

        string messageMap = result
            .Map(value => $"woo, we got ${value}!")
            .Match(value => value, error => $"boo, we got error number ${error.Code}!");

        Console.WriteLine(messageMap);
    }
}
