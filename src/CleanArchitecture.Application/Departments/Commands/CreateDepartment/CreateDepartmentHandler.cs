namespace CleanArchitecture.Application.Departments.Commands.CreateDepartment;

using AutoMapper;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Events;

using MediatR;

public class CreateDepartmentHandler 
    : IRequestHandler<CreateDepartmentCommand, CreateDepartmentDetailsDto>
{
    private readonly IDbContext context;
    private readonly IDateTimeService dateTimeService;
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    public CreateDepartmentHandler(
        IDbContext context, 
        IDateTimeService dateTimeService, 
        IMediator mediator, 
        IMapper mapper)
    {
        this.context = context;
        this.dateTimeService = dateTimeService;
        this.mediator = mediator;
        this.mapper = mapper;
    }

    public async Task<CreateDepartmentDetailsDto> Handle(
        CreateDepartmentCommand request, 
        CancellationToken cancellationToken)
    {
        var department = Department.Create(
            request.Name,
            this.dateTimeService.Now);

        await this.context.Departments.AddAsync(department, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        await this.mediator.Publish(
            new DepartmentCreatedEvent(department),
            cancellationToken);

        return this.mapper.Map<CreateDepartmentDetailsDto>(department);
    }
}
