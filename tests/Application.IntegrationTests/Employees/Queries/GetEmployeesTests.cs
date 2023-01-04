namespace Application.IntegrationTests.Employees.Queries;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees.Queries.GetEmployees;

using FluentAssertions;

public class GetEmployeesTests : BaseEmployeesTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private GetEmployeesHandler queryHandler;

    public GetEmployeesTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.queryHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.queryHandler = new GetEmployeesHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public void GetEmployeesHandlerIsCreatedCorrectly()
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

        var expected = this.Employees
            .AsQueryable()
            .ProjectTo<GetEmployeesListingDto>(this.mapper.ConfigurationProvider)
            .ToList();

        var query = new GetEmployeesQuery();

        var actual = (await this.queryHandler
                .Handle(query, cts.Token))
            .ToList();

        actual.Should().BeEquivalentTo(expected);
    }
}
