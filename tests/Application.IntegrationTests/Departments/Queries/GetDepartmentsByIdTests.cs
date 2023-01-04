namespace Application.IntegrationTests.Departments.Queries;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;
using CleanArchitecture.Domain.Entities;

using FluentAssertions;

public class GetDepartmentsByIdTests : BaseTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private GetDepartmentsByIdQueryHandler queryHandler;

    public GetDepartmentsByIdTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.queryHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.queryHandler = new GetDepartmentsByIdQueryHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public void GetDepartmentsByIdQueryHandlerIsCreatedCorrectly()
        => this.queryHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task GetByIdReturnsCorrectItem()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.mapper
            .Map<DepartmentFullDetailsDto>(department1);

        var query = new GetDepartmentsByIdQuery(department1.Id);

        var actual = await this.queryHandler
            .Handle(query, cts.Token);

        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdReturnsNullIfNoItemIsFound()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var query = new GetDepartmentsByIdQuery(-6);

        var actual = await this.queryHandler
            .Handle(query, cts.Token);

        actual.Should().BeNull();
    }
}
