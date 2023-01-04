namespace Application.IntegrationTests.Employees.Commands;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees.Commands.FireEmployee;

using FluentAssertions;
using FluentAssertions.Execution;

public class FireEmployeeTests : BaseEmployeesTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private FireEmployeeHandler commandHandler;

    public FireEmployeeTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.commandHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.commandHandler = new FireEmployeeHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public void FireEmployeeHandlerIsCreatedCorrectly()
        => this.commandHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task FireEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var department = this.CreateDepartment("IT");

        await this.context.Employees.AddRangeAsync(this.Employees, cts.Token);
        await this.context.Departments.AddAsync(department, cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var employee = this.Employees.First();
        var beforeFired = employee.IsActive;

        var command = new FireEmployeeCommand(employee.Id);

        var expected = await this.commandHandler.Handle(command, cts.Token);

        using (new AssertionScope())
        {
            beforeFired.Should().BeTrue();
            employee.IsActive.Should().BeFalse();

            expected.Should().NotBeNull();
            expected!.Name.Should().Be(employee.Name);
            expected.Department.Should().Be(department.Name);
        }
    }

    [Test]
    public async Task FireEmployeeShouldThrowsWithCanceledToken()
    {
        var cts = new CancellationTokenSource();

        await this.context.Employees.AddRangeAsync(this.Employees, cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.Employees.First();
        cts.Cancel();

        var command = new FireEmployeeCommand(expected.Id);

        using (new AssertionScope())
        {
            await FluentActions
                .Invoking(async () => await this.commandHandler.Handle(command, cts.Token))
                .Should()
                .ThrowAsync<OperationCanceledException>();

            expected.IsActive.Should().BeTrue();
        }
    }

    [Test]
    public async Task FireEmployeeReturnsNullWithInvalidId()
    {
        var command = new FireEmployeeCommand(-6);

        var expected = await this.commandHandler
            .Handle(command, CancellationToken.None);

        expected.Should().BeNull();
    }
}
