using CommonCommons.Models;
using FluentAssertions;
using Xunit;

namespace CommonCommonsTest.Models;

public class ResultTests
{
    [Fact]
    public void ResultTypeOperatesOnNullPath()
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
    public void ResultTypeOperatesOnSuccessPath()
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
}