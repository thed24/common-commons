using CommonCommons.Models;

namespace CommonCommonsTest.Common;

public enum Grade
{
    F = 0,
    D = 1,
    C = 2,
    B = 3,
    A = 4
}

public static class GradeExtensions
{
    public static Result<string> AsPhrase(this Grade? grade)
    {
        return grade switch
        {
            Grade.F => Result<string>.Success("Wow, you are a failure"),
            Grade.D => Result<string>.Success("You are not good enough"),
            Grade.C => Result<string>.Success("You are average"),
            Grade.B => Result<string>.Success("You are good"),
            Grade.A => Result<string>.Success("You are great"),
            _ => Result<string>.Failure("You can't even provide a correct grade")
        };
    }
}