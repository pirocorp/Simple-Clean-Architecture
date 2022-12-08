namespace Application.IntegrationTests.Services;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments;
using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.Entities;

using FluentAssertions;

public class DepartmentServiceTests : BaseServiceTests
{
    private readonly IMapper mapper;
    private readonly IDateTimeService dateTimeService;
    
    private IDbContext context;
    private IDepartmentService departmentService;

    public DepartmentServiceTests()
    {
        this.dateTimeService = this.GetDateTimeService();
        this.mapper = this.GetMapper();

        this.context = null!;
        this.departmentService = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.departmentService = new DepartmentService(
            this.context,
            this.dateTimeService,
            this.mapper);
    }

    [Test]
    public void DepartmentServiceIsCreatedCorrectly()
        => this.departmentService.Should().NotBeNull();

    [Test]
    public async Task GetByIdReturnsCorrectItem()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var actual = await this.departmentService
            .GetById(department1.Id);

        actual.Should().NotBeNull();
        actual.Id.Should().Be(department1.Id);
        actual.Name.Should().Be(department1.Name);
        actual.CreatedAt.Should().Be(department1.CreatedAt);
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

        var actual = await this.departmentService
            .GetById(-6);

        actual.Should().BeNull();
    }

    [Test]
    public async Task GetAllReturnsAllItems()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var result = await this.departmentService.GetAll();
        result.Count().Should().Be(2);
    }

    [Test]
    public async Task GetAllReturnsEmptyEnumerationIfNoItems()
    {
        var result = await this.departmentService.GetAll();
        result.Count().Should().Be(0);
    }

    [Test]
    public async Task CreateDepartmentCreatesItCorrectly()
    {
        var input = new CreateDepartmentDto
        {
            Name = "It"
        };

        var cts = new CancellationTokenSource();

        var response = await this.departmentService
            .CreateDepartment(input, cts.Token);

        response.Should().NotBeNull();
        response.Name.Should().Be(input.Name);
        response.CreatedAt.Should().Be(this.DateTime);
    }

    [Test]
    public async Task CreateDepartmentShouldThrowWithCanceledToken()
    {
        var input = new CreateDepartmentDto
        {
            Name = "It"
        };

        var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions.Invoking(async () 
            => await this.departmentService.CreateDepartment(input, cts.Token))
                .Should()
                .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public async Task RenameDepartmentWorksCorrectly()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var dto = new RenameDepartmentDto()
        {
            Name = "Executive"
        };

        department1.Name.Should().Be("IT");

        var response = await this.departmentService
            .RenameDepartment(department1.Id, dto, cts.Token);

        response.Should().NotBeNull();
        response?.Name.Should().Be("Executive");
    }

    [Test]
    public async Task RenameDepartmentShouldReturnNullIfDepartmentNotFound()
    {
        var cts = new CancellationTokenSource();

        var dto = new RenameDepartmentDto()
        {
            Name = "Executive"
        };

        var response = await this.departmentService
            .RenameDepartment(-5, dto, cts.Token);

        response.Should().BeNull();
    }

    [Test]
    public async Task RenameDepartmentShouldThrowWithCanceledToken()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        this.context.Departments.Add(department1);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var input = new RenameDepartmentDto()
        {
            Name = "It"
        };

        cts.Cancel();

        await FluentActions.Invoking(async () 
                => await this.departmentService.RenameDepartment(department1.Id, input, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public async Task DeleteDepartmentWorksCorrectly()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var response = await this.departmentService
            .DeleteDepartment(department1.Id, cts.Token);

        response.Should().NotBeNull();
        department1.IsActive.Should().BeFalse();
    }

    [Test]
    public async Task DeleteDepartmentReturnNullIfDepartmentNotFound()
    {
        var cts = new CancellationTokenSource();

        var dto = new RenameDepartmentDto()
        {
            Name = "Executive"
        };

        var response = await this.departmentService
            .DeleteDepartment(-5, cts.Token);

        response.Should().BeNull();
    }

    [Test]
    public async Task DeleteDepartmentShouldThrowWithCanceledToken()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        this.context.Departments.Add(department1);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var input = new RenameDepartmentDto()
        {
            Name = "It"
        };

        cts.Cancel();

        await FluentActions.Invoking(async () 
                => await this.departmentService.DeleteDepartment(department1.Id, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }
}
