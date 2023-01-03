namespace CleanArchitecture.Application.Common.Exceptions;

using System;

public class InvalidGenderException : ValidationException
{
    public InvalidGenderException(string propertyName)
        : base((propertyName, new [] { "Invalid gender" }))
    { }
}
