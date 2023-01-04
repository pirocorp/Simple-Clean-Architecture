namespace Application.IntegrationTests.Departments.Commands;

using System.ComponentModel.DataAnnotations;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments.Commands.CreateDepartment;
using CleanArchitecture.Domain.Events;

using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Moq;

using static CleanArchitecture.Domain.Common.DataConstants.Department;

public class CreateDepartmentCommandTests : BaseTests
{
    private readonly IMapper mapper;
    private readonly IDateTimeService dateTimeService;
    private readonly Mock<IMediator> mediator;

    private IDbContext context;
    private CreateDepartmentHandler commandHandler;

    public CreateDepartmentCommandTests()
    {
        this.dateTimeService = this.GetDateTimeService();
        this.mapper = this.GetMapper();
        this.mediator = this.GetMediator();

        this.context = null!;
        this.commandHandler = null!;
    }

    [SetUp]
    public void SetUp()
    {
        this.context = this.GetDatabase();
        this.commandHandler = new CreateDepartmentHandler(
            this.context,
            this.dateTimeService,
            this.mediator.Object,
            this.mapper);
    }

    [Test]
    public void CreateDepartmentCommandHandlerIsCreatedCorrectly()
        => this.commandHandler
            .Should()
            .NotBeNull();

    [Test]
    public async Task CreateDepartmentCreatesItCorrectly()
    {
        var input = new CreateDepartmentCommand
        {
            Name = "It"
        };

        var cts = new CancellationTokenSource();

        var actual = await this.commandHandler
            .Handle(input, cts.Token);

        using (new AssertionScope())
        {
            actual.Should().NotBeNull();
            actual.Id.Should().NotBe(0);
            actual.Name.Should().Be(input.Name);
            actual.CreatedAt.Should().Be(this.DateTime);

            this.mediator.Verify(
                x => x.Publish(
                    It.IsAny<DepartmentCreatedEvent>(), 
                cts.Token));
        }
    }

    [Test]
    public async Task CreateDepartmentShouldThrowWithCanceledToken()
    {
        var input = new CreateDepartmentCommand
        {
            Name = "It"
        };

        var cts = new CancellationTokenSource();
        cts.Cancel();

        await FluentActions
            .Invoking(
                async () => await this.commandHandler.Handle(input, cts.Token))
            .Should()
            .ThrowAsync<TaskCanceledException>();
    }

    [Test]
    public void CreateDepartmentCommandHasCorrectAttributes()
    {
        using (new AssertionScope())
        {
            this
                .TestPropertyForAttribute<RequiredAttribute, CreateDepartmentCommand>(
                    nameof(CreateDepartmentCommand.Name));

            var stringLengthAttribute = this
                .TestPropertyForAttribute<StringLengthAttribute, CreateDepartmentCommand>(
                    nameof(CreateDepartmentCommand.Name));

            stringLengthAttribute?.MaximumLength.Should().Be(NAME_MAX_LENGTH);
        }
    }
}
