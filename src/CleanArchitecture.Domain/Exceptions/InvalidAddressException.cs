namespace CleanArchitecture.Domain.Exceptions;

using System;

public class InvalidAddressException : Exception
{
    public InvalidAddressException(string message)
        : base($"Address is not valid. {message}")
    { }
}
