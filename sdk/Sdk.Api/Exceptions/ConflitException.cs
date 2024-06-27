namespace Sdk.Api.Exceptions;

public class ConflitException : SystemException
{
    public ConflitException(string? message)
        : base(message)
    {
    }

    public ConflitException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}