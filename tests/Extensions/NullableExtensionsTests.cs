using CommonCommons.Extensions;
using CommonCommons.Models;
using CommonCommonsTest.Common;
using FluentAssertions;
using Xunit;

namespace CommonCommonsTest.Extensions;

public class NullableExtensionsTests
{
    [Fact]
    public void GivenNullable_WhenMappingOnNullPath_ReturnNull()
    {
        // arrange
        int? value = null;

        // act
        int? result = value.Map(x => x + 1);

        // assert
        result.Should().Be(null);
    }
    
    [Fact]
    public void GivenNullable_WhenMappingOnValuePath_ReturnValue()
    {
        // arrange
        int? value = 1;
        
        // act
        int? result = value.Map(x => x + 1);
        
        // assert
        result.Should().Be(2);
    }

    [Theory]
    [InlineData(null, "No valid grade provided")]
    [InlineData("LOL", "No valid grade provided")]
    [InlineData("Z", "No valid grade provided")]
    [InlineData("49", "You can't even provide a correct grade")]
    [InlineData("B", "You are good")]
    [InlineData("A", "You are great")]
    public void GivenNullable_WhenMappingOnValuePathForReferenceType_ReturnValue(string grade, string expected)
    {
        // arrange
        User? user = User.From("Mark", grade);

        // act
        string result = user
            .Map<User, User>(currentUser => currentUser with { Name = "REDACTED"})
            .Map<User, Grade?>(currentUser => currentUser.AverageGrade)
            .Match<Grade, Result<string>>(
                success: currentGrade => currentGrade.AsPhrase(),
                failure: () => Result<string>.Failure("No valid grade provided"))
            .Match(
                success: phrase => phrase,
                failure: error => error);

        // assert
        result.Should().Be(expected);
    }
    
    [Fact]
    public async Task GivenNullable_WhenMappingOnNullPathAsync_ReturnNull()
    {
        // arrange
        int? value = null;

        // act
        int? result = await value.MapAsync(x => Task.FromResult(x + 1));

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GivenNullable_WhenMappingOnValuePathAsync_ReturnValue()
    {
        // arrange
        int? value = 1;

        // act
        int? result = await value.MapAsync(x => Task.FromResult(x + 1));

        // assert
        result.Should().Be(2);
    }

    [Fact]
    public async Task GivenNullable_WhenMatchingOnNullPath_ReturnNull()
    {
        // arrange
        int? value = null;

        // act
        int result = await value.MatchAsync(
            success: x => Task.FromResult(x),
            failure: () => Task.FromResult(-1));

        // assert
        result.Should().Be(-1);
    }

    [Fact]
    public async Task GivenNullable_WhenMatchingOnValuePath_ReturnValue()
    {
        // arrange
        int? value = 42;

        // act
        int result = await value.MatchAsync(
            success: x => Task.FromResult(x),
            failure: () => Task.FromResult(-1));

        // assert
        result.Should().Be(42);
    }
    
    [Theory]
    [InlineData("123", 123)]
    [InlineData("-123", -123)]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GivenNullable_WhenParsingInt_ReturnValue(string input, int? expected)
    {
        var result = input.TryParseToInt();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-123.45", -123.45)]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GivenNullable_WhenParsingDouble_ReturnValue(string input, double? expected)
    {
        var result = input.TryParseToDouble();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("123.45", 123.45)]
    [InlineData("-123.45", -123.45)]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GivenNullable_WhenParsingDecimal_ReturnValue(string input, double? expected)
    {
        var result = input.TryParseToDecimal();
        result.Should().Be((decimal?)expected);
    }

    [Theory]
    [InlineData("01/01/2020", "2020-01-01")]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GivenNullable_WhenParsingDateTime_ReturnValue(string input, string expectedString)
    {
        var expected = string.IsNullOrEmpty(expectedString) ? (DateTime?)null : DateTime.Parse(expectedString);
        var result = input.TryParseToDateTime();
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData("true", true)]
    [InlineData("false", false)]
    [InlineData("abc", null)]
    [InlineData(null, null)]
    public void GivenNullable_WhenParsingBool_ReturnValue(string input, bool? expected)
    {
        var result = input.TryParseToBool();
        result.Should().Be(expected);
    }
}
