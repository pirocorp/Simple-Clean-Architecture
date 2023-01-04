namespace Application.IntegrationTests.Employees.Commands;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees.Commands.UpdateEmployee;

using FluentAssertions;
using FluentAssertions.Execution;

using static CleanArchitecture.Domain.Common.DataConstants.Employee;

public class UpdateEmployeeTests : BaseEmployeesTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private UpdateEmployeeHandler commandHandler;

    public UpdateEmployeeTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.commandHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.commandHandler = new UpdateEmployeeHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public async Task UpdateEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var department = this.CreateDepartment("IT");
        var department2 = this.CreateDepartment("QA");

        await this.context.Employees.AddRangeAsync(this.Employees, cts.Token);
        await this.context.Departments.AddAsync(department, cts.Token);
        await this.context.Departments.AddAsync(department2, cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var command = new UpdateEmployeeCommand()
        {
            Id = 1,
            Address = "Saedinenie 16, 9701 Shumen, Shumen",
            Age = 35,
            DepartmentId = 2,
            Email = "new@new.new",
            Name = "NEW NEW",
            Salary = 125_000
        };

        var response = await this.commandHandler.Handle(command, cts.Token);

        using (new AssertionScope())
        {
            response.Should().NotBeNull();

            response!.Address.Should().Be(command.Address);
            response.Age.Should().Be(command.Age);
            response.DepartmentId.Should().Be(command.DepartmentId);
            response.Department.Should().Be(department2.Name);
            response.Email.Should().Be(command.Email);
            response.Name.Should().Be(command.Name);
            response.Salary.Should().Be(command.Salary);
        }
    }

    [Test]
    public async Task UpdateEmployeeReturnsNullWithInvalidId()
    {
        var cts = new CancellationTokenSource();
        var command = new UpdateEmployeeCommand()
        {
            Id = -51,
            Address = "ASD",
            Email = "asd@asd.com",
            Name = "Asd Asd"
        };

        var response = await this.commandHandler
            .Handle(command, cts.Token);

        response.Should().BeNull();
    }

    [Test]
    public void UpdateEmployeeCommandHasCorrectAttributes()
    {
        using var scope = new AssertionScope();

        this.TestPropertyForAttribute<RequiredAttribute, UpdateEmployeeCommand>(
            nameof(UpdateEmployeeCommand.Address));
        this.TestPropertyForAttribute<RequiredAttribute, UpdateEmployeeCommand>(
            nameof(UpdateEmployeeCommand.Email));
        this.TestPropertyForAttribute<RequiredAttribute, UpdateEmployeeCommand>(
            nameof(UpdateEmployeeCommand.Name));

        this.TestPropertyForAttribute<JsonIgnoreAttribute, UpdateEmployeeCommand>(
            nameof(UpdateEmployeeCommand.Id));

        this.TestPropertyForAttribute<EmailAddressAttribute, UpdateEmployeeCommand>(
            nameof(UpdateEmployeeCommand.Email));

        var addressStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, UpdateEmployeeCommand>(
                nameof(UpdateEmployeeCommand.Address));

        addressStringLengthAttribute.Should().NotBeNull();
        addressStringLengthAttribute!.MaximumLength.Should().Be(ADDRESS_MAX_LENGTH);

        var emailStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, UpdateEmployeeCommand>(
                nameof(UpdateEmployeeCommand.Email));

        emailStringLengthAttribute.Should().NotBeNull();
        emailStringLengthAttribute!.MaximumLength.Should().Be(EMAIL_MAX_LENGTH);

        var nameStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, UpdateEmployeeCommand>(
                nameof(UpdateEmployeeCommand.Name));

        nameStringLengthAttribute.Should().NotBeNull();
        emailStringLengthAttribute!.MaximumLength.Should().Be(EMAIL_MAX_LENGTH);

        var ageRangeAttribute = this
            .TestPropertyForAttribute<RangeAttribute, UpdateEmployeeCommand>(
                nameof(UpdateEmployeeCommand.Age));

        ageRangeAttribute.Should().NotBeNull();
        ageRangeAttribute!.Minimum.Should().Be(AGE_MIN_VALUE);
        ageRangeAttribute!.Maximum.Should().Be(AGE_MAX_VALUE);

        var salaryRangeAttribute = this
            .TestPropertyForAttribute<RangeAttribute, UpdateEmployeeCommand>(
                nameof(UpdateEmployeeCommand.Salary));

        salaryRangeAttribute.Should().NotBeNull();
        salaryRangeAttribute!.Minimum.Should().Be(SALARY_MIN_VALUE);
        salaryRangeAttribute!.Maximum.Should().Be(SALARY_MAX_VALUE);
    }
}
