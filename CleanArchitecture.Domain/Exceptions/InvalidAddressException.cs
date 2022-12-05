﻿namespace CleanArchitecture.Domain.Exceptions;

using System;

public class InvalidAddressException : Exception
{
    public InvalidAddressException()
        : base("Address is not valid.")
    { }
}
