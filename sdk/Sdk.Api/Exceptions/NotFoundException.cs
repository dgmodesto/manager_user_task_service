namespace Sdk.Api.Exceptions;

public class NotFoundException : SystemException
{
    public NotFoundException(string? message)
        : base(message)
    {
    }

    public NotFoundException(string? message, Exception? innerException)
        : base(message, innerException)
    {
    }
}