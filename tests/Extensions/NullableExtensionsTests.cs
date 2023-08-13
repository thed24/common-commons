using CommonCommons.Extensions;
using CommonCommons.Models;
using FluentAssertions;
using Xunit;

namespace CommonCommonsTest.Extensions;

public class NullableExtensionsTests
{
    [Fact]
    public void NullableMapOperatesOnNullPath()
    {
        // arrange
        int? value = null;
        
        // act
        int? result = value.Map(x => x + 1);
        
        // assert
        result.Should().Be(null);
    }
    
    [Fact]
    public void NullableMapOperatesOnNonNullPath()
    {
        // arrange
        int? value = 1;
        
        // act
        int? result = value.Map(x => x + 1);
        
        // assert
        result.Should().Be(2);
    }
    
    [Fact]
    public void NullableMapOperatesOnReferenceTypes()
    {
        // arrange
        User? user = UserFromString("Mark", "A");
            
        // act
        string result = user
            .Map(ExtractGrade)
            .Match(
                success: IsAverageGrade,
                failure: HandleError)
            .Match(
                success: PrintGrade,
                failure: PrintError);
        
        // assert
        result.Should().Be("The grade is not average");

        // local helper funcs for transforming data
        static User? UserFromString(string name, string grade)
        {
            Grade? parsedGrade = Enum.TryParse(grade, out Grade maybeGrade) ? maybeGrade : null;
            return parsedGrade is null ? null : new User(name, parsedGrade.Value);
        }

        static Result<bool, string> IsAverageGrade(int grade)
        {
            return Result<bool, string>.Success(grade is >= 60 and <= 80);
        }

        static Result<bool, string> HandleError(int _)
        {
            return Result<bool, string>.Failure("Could not parse users grade.");
        }

        static string PrintGrade(bool isAverage)
        {
            string modifier = isAverage ? "is" : "is not";
            return $"The grade {modifier} average";
        }

        static string PrintError(string error)
        {
            return error;
        }

        static int ExtractGrade(User user)
            {
                return user.Grade switch
                {
                    Grade.F => 50,
                    Grade.D => 60,
                    Grade.C => 70,
                    Grade.B => 80,
                    Grade.A => 90,
                    _ => 0
                };
            }
    }
    
    #region Classes for testing
    private record User(string Name, Grade Grade);

    private enum Grade
    {
        F = 0,
        D = 1,
        C = 2,
        B = 3,
        A = 4
    }
    #endregion
}