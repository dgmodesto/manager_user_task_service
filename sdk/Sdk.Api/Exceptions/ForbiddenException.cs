﻿namespace Sdk.Api.Exceptions;

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Forbidden")
    {
    }
}