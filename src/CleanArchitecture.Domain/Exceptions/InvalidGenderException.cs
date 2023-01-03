namespace CleanArchitecture.Domain.Exceptions;

using System;

public class InvalidGenderException : Exception
{
    public InvalidGenderException(string propertyName)
        : base("Invalid gender")
    { }
}
