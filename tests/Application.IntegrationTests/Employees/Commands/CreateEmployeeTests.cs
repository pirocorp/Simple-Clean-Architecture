namespace Application.IntegrationTests.Employees.Commands;

using System.ComponentModel.DataAnnotations;

using AutoMapper;

using CleanArchitecture.Application.Common.Exceptions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees.Commands.CreateEmployee;

using FluentAssertions;
using FluentAssertions.Execution;

using static CleanArchitecture.Domain.Common.DataConstants.Employee;

public class CreateEmployeeTests : BaseEmployeesTests
{
    private readonly IDateTimeService dateTimeService;
    private readonly IMapper mapper;

    private IDbContext context;
    private CreateEmployeeHandler commandHandler;

    public CreateEmployeeTests()
    {
        this.dateTimeService = this.GetDateTimeService();
        this.mapper = this.GetMapper();

        this.context = null!;
        this.commandHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.commandHandler = new CreateEmployeeHandler(
            this.context,
            this.dateTimeService,
            this.mapper);
    }

    [Test]
    public void CreateEmployeeCommandHandlerIsCreatedCorrectly()
        => this.commandHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task CreateEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var department = this.CreateDepartment("IT");

        await this.context.Departments.AddAsync(department, cts.Token);

        var command = new CreateEmployeeCommand
        {
            Address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
            Age = 35,
            DepartmentId = 1,
            Email = "postakalka@abv.bg",
            Gender = "Male",
            Name = "Zdravko Zdravkov",
            Salary = 25_000,
        };

        var actual = await this.commandHandler.Handle(command, cts.Token);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBe(0);
            actual.Name.Should().Be(command.Name);
            actual.Department.Should().Be(department.Name);
        }
    }

    [Test]
    public async Task CreateEmployeeThrowsWithInvalidGender()
    {
        var command = new CreateEmployeeCommand
        {
            Address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
            Age = 35,
            DepartmentId = 1,
            Email = "postakalka@abv.bg",
            Gender = "INVALID",
            Name = "Zdravko Zdravkov",
            Salary = 25_000,
        };

        await FluentActions
            .Invoking(async () => await this.commandHandler.Handle(command, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidGenderException>();
    }

    [Test]
    public async Task CreateEmployeeShouldThrowWithCanceledToken()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var command = new CreateEmployeeCommand
        {
            Address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
            Age = 35,
            DepartmentId = 1,
            Email = "postakalka@abv.bg",
            Gender = "Male",
            Name = "Zdravko Zdravkov",
            Salary = 25_000,
        };

        await FluentActions
            .Invoking(async () => await this.commandHandler.Handle(command, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public void CreateEmployeeCommandHasCorrectAttributes()
    {
        using var scope = new AssertionScope();

        this.TestPropertyForAttribute<RequiredAttribute, CreateEmployeeCommand>(
            nameof(CreateEmployeeCommand.Address));
        this.TestPropertyForAttribute<RequiredAttribute, CreateEmployeeCommand>(
            nameof(CreateEmployeeCommand.Email));
        this.TestPropertyForAttribute<RequiredAttribute, CreateEmployeeCommand>(
            nameof(CreateEmployeeCommand.Name));

        this.TestPropertyForAttribute<EmailAddressAttribute, CreateEmployeeCommand>(
            nameof(CreateEmployeeCommand.Email));

        var addressStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, CreateEmployeeCommand>(
                nameof(CreateEmployeeCommand.Address));

        addressStringLengthAttribute.Should().NotBeNull();
        addressStringLengthAttribute?.MaximumLength.Should().Be(ADDRESS_MAX_LENGTH);

        var emailStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, CreateEmployeeCommand>(
                nameof(CreateEmployeeCommand.Email));

        emailStringLengthAttribute.Should().NotBeNull();
        emailStringLengthAttribute?.MaximumLength.Should().Be(EMAIL_MAX_LENGTH);

        var nameStringLengthAttribute = this
            .TestPropertyForAttribute<StringLengthAttribute, CreateEmployeeCommand>(
                nameof(CreateEmployeeCommand.Name));

        nameStringLengthAttribute.Should().NotBeNull();
        nameStringLengthAttribute?.MaximumLength.Should().Be(NAME_MAX_LENGTH);

        var ageRangeAttribute = this
            .TestPropertyForAttribute<RangeAttribute, CreateEmployeeCommand>(
                nameof(CreateEmployeeCommand.Age));

        ageRangeAttribute.Should().NotBeNull();
        ageRangeAttribute.Minimum.Should().Be(AGE_MIN_VALUE);
        ageRangeAttribute.Maximum.Should().Be(AGE_MAX_VALUE);

        var salaryRangeAttribute = this
            .TestPropertyForAttribute<RangeAttribute, CreateEmployeeCommand>(
                nameof(CreateEmployeeCommand.Salary));

        salaryRangeAttribute.Should().NotBeNull();
        salaryRangeAttribute.Minimum.Should().Be(SALARY_MIN_VALUE);
        salaryRangeAttribute.Maximum.Should().Be(SALARY_MAX_VALUE);
    }
}
