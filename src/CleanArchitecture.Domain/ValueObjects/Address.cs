namespace CleanArchitecture.Domain.ValueObjects;

using System;
using System.Collections.Generic;

using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Exceptions;

public class Address : ValueObject
{
    public Address(
        string municipality, 
        int postCode, 
        string province, 
        string street, 
        int streetNumber)
    {
        if (postCode < 0 || streetNumber < 0)
        {
            throw new InvalidAddressException("Post code or street number cannot be negative.");
        }

        Municipality = municipality;
        PostCode = postCode;
        Province = province;
        Street = street;
        StreetNumber = streetNumber;
    }

    public string Municipality { get; set; }

    public int PostCode { get; set; }

    public string Province { get; set; }

    public string Street { get; set; }

    public int StreetNumber { get; set; }

    public static Address Empty
        => new (
            string.Empty, 
            0, 
            string.Empty, 
            string.Empty, 
            0);

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return this.Municipality;

        yield return this.PostCode;

        yield return this.Province;

        yield return this.Street;

        yield return this.StreetNumber;
    }

    public override string ToString()
    {
        return $"{this.Street} {this.StreetNumber}, {this.PostCode} {this.Municipality}, {this.Province}";
    }

    public static Address From(string address)
    {
        var tokens = address
            .Split(", ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        if (tokens.Length != 5)
        {
            throw new InvalidAddressException("Missing information to form complete address.");
        }

        var street = tokens[0];
        var validStreetNumber = int.TryParse(tokens[1], out var streetNumber);
        var validPostCode = int.TryParse(tokens[2], out var postCode);
        var municipality = tokens[3];
        var province = tokens[4];

        if (!validStreetNumber || validPostCode)
        {
            throw new InvalidAddressException("Invalid street number or post code.");
        }

        return new Address(municipality, postCode, province, street, streetNumber);
    }
}
