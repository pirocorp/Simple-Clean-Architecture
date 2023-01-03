namespace CleanArchitecture.Application.Departments.Commands.RenameDepartment;

using AutoMapper;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class RenameDepartmentHandler
    : IRequestHandler<RenameDepartmentCommand, RenameDepartmentDetailsDto?>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public RenameDepartmentHandler(IDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<RenameDepartmentDetailsDto?> Handle(
        RenameDepartmentCommand request, 
        CancellationToken cancellationToken)
    {
        var department = await this.context.Departments
            .Where(d => d.IsActive)
            .FirstOrDefaultAsync(d => d.Id == request.Id, cancellationToken);

        if (department is null)
        {
            return null;
        }

        department.Rename(request.Name);
        await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<RenameDepartmentDetailsDto>(department);
    }
}
