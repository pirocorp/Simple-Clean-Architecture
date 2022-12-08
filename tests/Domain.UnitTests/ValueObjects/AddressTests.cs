namespace Domain.UnitTests.ValueObjects;

using CleanArchitecture.Domain.Exceptions;
using CleanArchitecture.Domain.ValueObjects;
using FluentAssertions;

public class AddressTests
{
    [Test]
    public void PropertiesAreInitializedCorrectly()
    {
        var municipality = "Strombeek-Bever";
        var postCode = 1853;
        var province = "Grimbergen";
        var street = "Valkendal";
        var streetNumber = 8;

        var address = new Address(
            municipality,
            postCode,
            province,
            street,
            streetNumber);

        address.Municipality.Should().Be(municipality);
        address.PostCode.Should().Be(postCode);
        address.Province.Should().Be(province);
        address.Street.Should().Be(street);
        address.StreetNumber.Should().Be(streetNumber);
    }

    [Theory]
    [TestCase(-1)]
    [TestCase(-1853)]
    public void ConstructorShouldThrowWithLessThanZeroPostCodeOrStreetNumber(int postCode)
    {
        var municipality = "Strombeek-Bever";
        var province = "Grimbergen";
        var street = "Valkendal";
        var streetNumber = 8;

        Constructor(() => new Address(municipality, postCode, province, street, streetNumber))
            .Should()
            .Throw<InvalidAddressException>();
    }

    [Theory]
    [TestCase(-1)]
    [TestCase(-1853)]
    public void ConstructorShouldThrowWithLessThenZeroStreetNumber(int streetNumber)
    {
        var municipality = "Strombeek-Bever";
        var province = "Grimbergen";
        var street = "Valkendal";
        var postCode = 1853;

        Constructor(() => new Address(municipality, postCode, province, street, streetNumber))
            .Should()
            .Throw<InvalidAddressException>();
    }

    [Test]
    public void EqualsWorksCorrectly()
    {
        var municipality = "Strombeek-Bever";
        var postCode = 1853;
        var province = "Grimbergen";
        var street = "Valkendal";
        var streetNumber = 8;

        var address = new Address(
            municipality,
            postCode,
            province,
            street,
            streetNumber);

        var expected = new Address(
            municipality,
            postCode,
            province,
            street,
            streetNumber);

        var result = address.Equals(expected);
        result.Should().BeTrue();
    }

    [Test]
    public void ToStringShouldReturnAddressInCorrectFormat()
    {
        var expected = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen";

        var address = new Address(
            "Strombeek-Bever",
            1853,
            "Grimbergen",
            "Valkendal",
            8);

        address.ToString().Should().Be(expected);
    }

    [Theory]
    [TestCase("Valkendal 8, 1853 Strombeek-Bever, Grimbergen")]
    [TestCase("Valkendal 8 1853 Strombeek-Bever, Grimbergen")]
    [TestCase("Valkendal 8, 1853 Strombeek-Bever Grimbergen")]
    [TestCase("Valkendal 8 1853 Strombeek-Bever Grimbergen")]
    [TestCase("Valkendal, 8, 1853, Strombeek-Bever, Grimbergen")]
    public void AddressFromShouldWorkCorrectly(string input)
    {
        var municipality = "Strombeek-Bever";
        var postCode = 1853;
        var province = "Grimbergen";
        var street = "Valkendal";
        var streetNumber = 8;

        var address = Address.From(input);

        address.Municipality.Should().Be(municipality);
        address.PostCode.Should().Be(postCode);
        address.Province.Should().Be(province);
        address.Street.Should().Be(street);
        address.StreetNumber.Should().Be(streetNumber);
    }

    [Theory]
    [TestCase("Valkendal 8, 1853 Strombeek-Bever")]
    [TestCase("Valkendal Eight, 1853 Strombeek-Bever, Grimbergen")]
    [TestCase("Valkendal 8, Eight Strombeek-Bever, Grimbergen")]
    public void AddressFromShouldThrow(string input)
    {
        FluentActions
            .Invoking(() => Address.From(input))
            .Should()
            .Throw<InvalidAddressException>();
    }

    private static Action Constructor<T>(Func<T> func)
    {
        return () => func();
    }
}
