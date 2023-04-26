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
        return new Result<TValue, TError> {Value = value};
    }
    
    public static Result<TValue, TError> Failure(TError error)
    {
        return new Result<TValue, TError> {Error = error};
    }
    
    public Result<TValue2, TError> Map<TValue2>(Func<TValue, TValue2> map)
    {
        return IsSuccess 
            ? Result<TValue2, TError>.Success(map(Value!)) 
            : Result<TValue2, TError>.Failure(Error);
    }
    
    public async Task<Result<TValue2, TError>> MapAsync<TValue2>(Func<TValue, Task<TValue2>> map)
    {
        return IsSuccess 
            ? Result<TValue2, TError>.Success(await map(Value!)) 
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
