namespace ManagerUserTaskApi.Domain.Extensions;

public static class StringExtensions
{
    public static string? Capitalize(this string? input)
    {
        return input switch
        {
            null => null,
            "" => "",
            _ => string.Concat(input[0].ToString().ToUpper(), input.AsSpan(1))
        };
    }
}