using CommonCommons.Extensions;
using CommonCommons.Models;
using FluentAssertions;
using Xunit;

namespace CommonCommonsTest.Models;

public class ResultTests
{
    [Fact]
    public void GivenResult_WhenMappingOnNullPath_ReturnFallback()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Failure("kernel went kablooey");
        
        // act
        string actual = result
            .Map(value => "Woo, we got " + value + "!")
            .Match(
                success: value => value,
                failure: error => "Boo, we got some error: " + error + "!");

        // assert
        actual.Should().Be("Boo, we got some error: kernel went kablooey!");
    }
    
    [Fact]
    public void GivenResult_WhenMappingOnValuePath_ReturnValue()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Success(42);
        
        // act
        string actual = result
            .Map(value => "Woo, we got " + value + "!")
            .Match(
                success: value => value,
                failure: error => "Boo, we got some error: " + error + "!");

        // assert
        actual.Should().Be("Woo, we got 42!");
    }
    
    [Fact]
    public async Task GivenResult_WhenMappingOnNullPathAsync_ReturnFallback()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Success(42);
        
        // act
        string actual = await result
            .MapAsync<string>(value => Task.FromResult("Woo, we got " + value + "!"))
            .MatchAsync<string, string, string>(
                success: value => Task.FromResult(value),
                failure: error => Task.FromResult("Boo, we got some error: " + error + "!"));

        // assert
        actual.Should().Be("Woo, we got 42!");
    }

    [Fact]
    public async Task GivenResult_WhenMappingOnValuePathAsync_ReturnValue()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Failure("kernel went kablooey");
        
        // act
        string actual = await result
            .MapAsync(value => Task.FromResult("Woo, we got " + value + "!"))
            .MatchAsync(
                success: value => Task.FromResult(value),
                failure: error => Task.FromResult("Boo, we got some error: " + error + "!"));

        // assert
        actual.Should().Be("Boo, we got some error: kernel went kablooey!");
    }

    [Fact]
    public void GivenResult_WhenDoOnValuePath_DoAction()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Success(42);
        string actual = string.Empty;

        // act
        result.Do(
            success: value => actual = "Woo, we got " + value + "!",
            failure: error => actual = "Boo, we got some error: " + error + "!");

        // assert
        actual.Should().Be("Woo, we got 42!");
    }

    [Fact]
    public void GivenResult_WhenDoOnNullPath_DoFallback()
    {
        // arrange
        Result<int, string> result = Result<int, string>.Failure("kernel went kablooey");
        string actual = string.Empty;

        // act
        result.Do(
            success: value => actual = "Woo, we got " + value + "!",
            failure: error => actual = "Boo, we got some error: " + error + "!");

        // assert
        actual.Should().Be("Boo, we got some error: kernel went kablooey!");
    }
}