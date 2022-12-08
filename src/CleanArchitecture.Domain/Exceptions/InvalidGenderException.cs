namespace CleanArchitecture.Domain.Exceptions;

using System;

public class InvalidGenderException : Exception
{
    public InvalidGenderException()
        : base("Invalid gender")
    { }
}
