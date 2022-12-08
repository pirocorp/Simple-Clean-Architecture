namespace Application.IntegrationTests.Services;

using System.Linq.Expressions;
using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

using Moq;

public abstract class BaseServiceTests
{
    protected readonly DateTime DateTime;

    protected BaseServiceTests()
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
}
