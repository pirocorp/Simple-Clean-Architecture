namespace Application.IntegrationTests.Services;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;

using FluentAssertions;

public class EmployeeServiceTests : BaseServiceTests
{
    private readonly IMapper mapper;
    private readonly IDateTimeService dateTimeService;
    
    private IDbContext context;
    private IDepartmentService departmentService;
    private IEmployeeService employeeService;
    private List<Employee> employees;

    public EmployeeServiceTests()
    {
        this.dateTimeService = this.GetDateTimeService();
        this.mapper = this.GetMapper();

        this.context = null!;
        this.departmentService = null!;
        this.employeeService = null!;
        this.employees = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();

        this.departmentService = new DepartmentService(
            this.context,
            this.dateTimeService,
            this.mapper);

        this.employeeService = new EmployeeService(
            this.context,
            this.dateTimeService,
            this.departmentService,
            this.mapper);

        this.InitializeEmployees();
    }

    [Test]
    public void DepartmentServiceIsCreatedCorrectly()
        => this.departmentService.Should().NotBeNull();

    [Test]
    public void EmployeeServiceIsCreatedCorrectly()
        => this.employeeService.Should().NotBeNull();

    [Test]
    public async Task GetByIdWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var departmentName = "It";

        await this.context.Employees.AddRangeAsync(this.employees, cts.Token);
        await this.context.Departments.AddAsync(Department.Create(departmentName, DateTime.Now), cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.employees.First();
        var actual = await this.employeeService
            .GetById(expected.Id);

        actual.Should().NotBeNull();
        actual?.Address.ToString().Should().Be(expected.Address.ToString());
        actual?.Department.Should().Be(departmentName);
        actual?.DepartmentId.Should().Be(expected.DepartmentId);
        actual?.Name.Should().Be(expected.Name);
        actual?.Gender.ToString().Should().Be(expected.Gender.ToString());
        actual?.Age.Should().Be(expected.Age);
        actual?.CreatedAt.Should().Be(expected.CreatedAt);
        actual?.Email.Should().Be(expected.Email);
        actual?.Salary.Should().Be(expected.Salary);
    }

    [Test]
    public async Task GetByIdShouldReturnNullWithIdIsMissing()
    {
        var cts = new CancellationTokenSource();

        var actual = await this.employeeService.GetById(-6);

        actual.Should().BeNull();
    }

    [Test]
    public async Task GetAllWorksCorrectly()
    {
        var cts = new CancellationTokenSource();

        await this.context.Employees.AddRangeAsync(this.employees, cts.Token);
        await this.context.Departments.AddAsync(Department.Create("IT", DateTime.Now), cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var actual = (await this.employeeService.GetAll()).ToList();

        actual.Should().NotBeNull();
        actual.Count.Should().Be(this.employees.Count);
    }

    [Test]
    public async Task CreateEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var departmentName = "It";

        await this.context.Departments
            .AddAsync(Department.Create(departmentName, this.DateTime), cts.Token);

        var dto = new CreateEmployeeDto
        {
            Address = "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
            Age = 35,
            DepartmentId = 1,
            Email = "postakalka@abv.bg",
            Gender = "Male",
            Name = "Zdravko Zdravkov",
            Salary = 25_000,
        };

        var response = await this.employeeService.CreateEmployee(dto, cts.Token);

        response.Should().NotBeNull();
        response.Name.Should().Be(dto.Name);
        response.Age.Should().Be(dto.Age);
        response.Address.Should().Be(dto.Address);
        response.CreatedAt.Should().Be(this.DateTime);
        response.Department.Should().Be(departmentName);
        response.DepartmentId.Should().Be(dto.DepartmentId);
        response.Salary.Should().Be(dto.Salary);
        response.Gender.Should().Be(dto.Gender);
        response.Email.Should().Be(dto.Email);
    }

    [Test]
    public async Task CreateEmployeeThrowsWithInvalidGender()
    {
        var dto = new CreateEmployeeDto
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
            .Invoking(async() => await this.employeeService.CreateEmployee(dto, CancellationToken.None))
            .Should()
            .ThrowAsync<InvalidGenderException>();
    }

    [Test]
    public async Task CreateEmployeeShouldThrowWithCanceledToken()
    {
        var cts = new CancellationTokenSource();
        cts.Cancel();

        var dto = new CreateEmployeeDto
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
            .Invoking(async() => await this.employeeService.CreateEmployee(dto, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public async Task FireEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var departmentName = "It";

        await this.context.Employees.AddRangeAsync(this.employees, cts.Token);
        await this.context.Departments.AddAsync(Department.Create(departmentName, DateTime.Now), cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.employees.First();

        expected.IsActive.Should().BeTrue();

        await this.employeeService.FireEmployee(expected.Id, cts.Token);

        expected.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task FireEmployeeShouldThrowsWithCanceledToken()
    {
        var cts = new CancellationTokenSource();
        var departmentName = "It";

        await this.context.Employees.AddRangeAsync(this.employees, cts.Token);
        await this.context.Departments.AddAsync(Department.Create(departmentName, DateTime.Now), cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var expected = this.employees.First();
        cts.Cancel();

        await FluentActions.Invoking(async () => await this.employeeService.FireEmployee(expected.Id, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public async Task FireEmployeeReturnsNullWithInvalidId()
    {
        var expected = await this.employeeService.FireEmployee(-6, CancellationToken.None);

        expected.Should().BeNull();
    }

    [Test]
    public async Task UpdateEmployeeWorksCorrectly()
    {
        var cts = new CancellationTokenSource();
        var departmentName = "It";

        await this.context.Employees.AddRangeAsync(this.employees, cts.Token);
        await this.context.Departments.AddAsync(Department.Create(departmentName, DateTime.Now), cts.Token);
        await this.context.SaveChangesAsync(cts.Token);

        var dto = new UpdateEmployeeDto
        {
            Address = "Saedinenie 16, 9701 Shumen, Shumen",
            Age = 35,
            DepartmentId = 1,
            Email = "asd@asd.com",
            Name = "Piroman Piromanov",
            Salary = 125_000
        };

        var response = await this.employeeService.Update(1, dto, cts.Token);

        response.Should().NotBeNull();
        response?.Address.Should().Be(dto.Address);
        response?.Age.Should().Be(dto.Age);
        response?.DepartmentId.Should().Be(dto.DepartmentId);
        response?.Department.Should().Be(departmentName);
        response?.Email.Should().Be(dto.Email);
        response?.Name.Should().Be(dto.Name);
        response?.Salary.Should().Be(dto.Salary);
    }

    [Test]
    public async Task UpdateEmployeeReturnsNullWithInvalidId()
    {
        var cts = new CancellationTokenSource();
        var dto = new UpdateEmployeeDto();

        var response = await this.employeeService.Update(-51, dto, cts.Token);

        response.Should().BeNull();
    }

    private void InitializeEmployees()
    {
        this.employees = new List<Employee>
        {
            Employee.Create(
                "Zdravko Zdravkov",
                35,
                DateTime.Now,
                "postakalka@abv.bg",
                "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
                Gender.Male,
                25_000,
                1),

            Employee.Create(
                "Asen Zlatarov",
                35,
                DateTime.Now,
                "postakalka2@abv.bg",
                "Valkendal 8, 1853 Strombeek-Bever, Grimbergen",
                Gender.Male,
                25_000,
                1)
        };
    }
}
