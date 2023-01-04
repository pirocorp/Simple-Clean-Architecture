namespace Application.IntegrationTests.Employees.Queries;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees.Queries.GetEmployeesById;

using FluentAssertions;

public class GetEmployeesByIdTests : BaseEmployeesTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private GetEmployeesByIdHandler queryHandler;

    public GetEmployeesByIdTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.queryHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.queryHandler = new GetEmployeesByIdHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public void GetEmployeesByIdHandlerIsCreatedCorrectly()
        => this.queryHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task GetByIdWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var department = this.CreateDepartment("IT");

        await this.context.Employees.AddRangeAsync(this.Employees, cts.Token);
        await this.context.Departments.AddAsync(department, cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.mapper.Map<GetEmployeesByIdDto>(this.Employees.First());

        var query = new GetEmployeesByIdQuery(expected.Id);

        var actual = await this.queryHandler
            .Handle(query, cts.Token);

        actual.Should().NotBeNull();
        actual.Should().BeEquivalentTo(expected);
    }

    [Test]
    public async Task GetByIdShouldReturnNullWithIdIsMissing()
    {
        var cts = new CancellationTokenSource();
        var query = new GetEmployeesByIdQuery(-6);

        var actual = await this.queryHandler
            .Handle(query, cts.Token);

        actual.Should().BeNull();
    }
}
