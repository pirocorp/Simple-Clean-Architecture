namespace Application.IntegrationTests.Departments.Commands;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments.Commands.RenameDepartment;
using CleanArchitecture.Domain.Entities;

using FluentAssertions;
using FluentAssertions.Execution;

using static CleanArchitecture.Domain.Common.DataConstants.Department;

public class RenameDepartmentCommandTests : BaseTests
{
    private readonly IMapper mapper;
    
    private IDbContext context;
    private RenameDepartmentHandler commandHandler;

    public RenameDepartmentCommandTests()
    {
        this.mapper = this.GetMapper();

        this.context = null!;
        this.commandHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.commandHandler = new RenameDepartmentHandler(
            this.context,
            this.mapper);
    }

    [Test]
    public void RenameDepartmentCommandHandlerIsCreatedCorrectly()
        => this.commandHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task RenameDepartmentWorksCorrectly()
    {
        var department1 = Department.Create("IT", DateTime.Now);
        var department2 = Department.Create("HR", DateTime.Now);

        this.context.Departments.Add(department1);
        this.context.Departments.Add(department2);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var newName = "Executive";
        var command = new RenameDepartmentCommand()
        {
            Id = department1.Id,
            Name = newName
        };

        department1.Name.Should().Be(department1.Name);

        var actual = await this.commandHandler
            .Handle(command, cts.Token);

        actual.Should().NotBeNull();
        actual?.Name.Should().Be(newName);
    }

    [Test]
    public async Task RenameDepartmentShouldReturnNullIfDepartmentNotFound()
    {
        var cts = new CancellationTokenSource();

        var command = new RenameDepartmentCommand()
        {
            Id = -5,
            Name = "Executive"
        };

        var response = await this.commandHandler
            .Handle(command, cts.Token);

        response.Should().BeNull();
    }

    [Test]
    public async Task RenameDepartmentShouldThrowWithCanceledToken()
    {
        var department1 = Department.Create("IT", DateTime.Now);

        this.context.Departments.Add(department1);

        var cts = new CancellationTokenSource();
        await this.context.SaveChangesAsync(cts.Token);

        var command = new RenameDepartmentCommand()
        {
            Name = "It"
        };

        cts.Cancel();

        await FluentActions.Invoking(async () 
                => await this.commandHandler.Handle(command, cts.Token))
            .Should()
            .ThrowAsync<OperationCanceledException>();
    }

    [Test]
    public void RenameDepartmentCommandHasCorrectAttributes()
    {
        using (new AssertionScope())
        {
            this.TestPropertyForAttribute<JsonIgnoreAttribute, RenameDepartmentCommand>(
                nameof(RenameDepartmentCommand.Id));

            this.TestPropertyForAttribute<RequiredAttribute, RenameDepartmentCommand>(
                nameof(RenameDepartmentCommand.Name));

            var stringLengthAttribute = this
                .TestPropertyForAttribute<StringLengthAttribute, RenameDepartmentCommand>(
                    nameof(RenameDepartmentCommand.Name));

            stringLengthAttribute.Should().NotBeNull();
            stringLengthAttribute?.MaximumLength.Should().Be(NAME_MAX_LENGTH);
        }
    }
}
