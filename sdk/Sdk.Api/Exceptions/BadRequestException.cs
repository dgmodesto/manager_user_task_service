namespace Sdk.Api.Exceptions;

public class BadRequestException : SystemException
{
    public BadRequestException(string? message)
        : base(message)
    {
    }

    public BadRequestException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}