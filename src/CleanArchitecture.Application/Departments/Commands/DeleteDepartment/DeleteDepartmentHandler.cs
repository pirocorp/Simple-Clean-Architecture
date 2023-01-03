namespace CleanArchitecture.Application.Departments.Commands.DeleteDepartment;

using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class DeleteDepartmentHandler
    : IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentDetailsDto?>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;
    private readonly IMediator mediator;

    public DeleteDepartmentHandler(
        IDbContext context, 
        IMapper mapper, 
        IMediator mediator)
    {
        this.context = context;
        this.mapper = mapper;
        this.mediator = mediator;
    }

    public async Task<DeleteDepartmentDetailsDto?> Handle(
        DeleteDepartmentCommand request, 
        CancellationToken cancellationToken)
    {
        var department = await this.context.Departments
            .Where(d => d.IsActive)
            .FirstOrDefaultAsync(
                d => d.Id == request.Id,
                cancellationToken);

        if (department is null)
        {
            return null;
        }

        department.Delete();
        await this.context.SaveChangesAsync(cancellationToken);

        await this.mediator.Publish(
            new DepartmentDeletedEvent(department),
            cancellationToken);

        return this.mapper.Map<DeleteDepartmentDetailsDto>(department);
    }
}
