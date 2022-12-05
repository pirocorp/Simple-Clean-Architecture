namespace CleanArchitecture.Domain.ValueObjects;

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
            throw new InvalidAddressException();
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
}
