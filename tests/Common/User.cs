namespace CommonCommonsTest.Common;

public record User(string Name, Grade AverageGrade)
{
    public static User? From(string name, string grade)
    {
        Grade? parsedGrade = Enum.TryParse(grade, out Grade maybeGrade) ? maybeGrade : null;

        return parsedGrade is null 
            ? null 
            : new User(name, parsedGrade.Value);
    }
}
