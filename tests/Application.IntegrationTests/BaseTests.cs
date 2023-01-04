namespace Application.IntegrationTests;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Persistence;

using FluentAssertions;

using MediatR;

using Microsoft.EntityFrameworkCore;

using Moq;

public abstract class BaseTests
{
    protected readonly DateTime DateTime;

    protected BaseTests()
    {
        this.DateTime = DateTime.UtcNow;
    }

    protected IMapper GetMapper()
    {
        var configuration = new MapperConfiguration(
            config
                => config.AddProfile<MappingProfile>());

        return configuration.CreateMapper();
    }

    protected IDateTimeService GetDateTimeService()
    {
        var moq = new Mock<IDateTimeService>();

        moq.Setup(x => x.Now)
            .Returns(this.DateTime);

        return moq.Object;
    }

    protected IDbContext GetDatabase()
    {
        var dbOptions = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(dbOptions);
    }

    protected Mock<IMediator> GetMediator() => new ();

    protected TAttribute TestPropertyForAttribute<TAttribute, TClass>(string propertyName)
        where TAttribute : Attribute
    {
        var type = typeof(TClass);
        var property = type.GetProperty(propertyName);

        property.Should().NotBeNull();

        var attributeIsDefined = Attribute.IsDefined(property!, typeof(TAttribute));

        attributeIsDefined.Should().BeTrue();

        return (TAttribute)Attribute.GetCustomAttribute(property!, typeof(TAttribute))!;
    }
}
