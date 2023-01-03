namespace CleanArchitecture.Application.Employees.Commands.FireEmployee;

using AutoMapper;
using Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class FireEmployeeHandler
    : IRequestHandler<FireEmployeeCommand, FireEmployeeDto?>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public FireEmployeeHandler(IDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<FireEmployeeDto?> Handle(
        FireEmployeeCommand request, 
        CancellationToken cancellationToken)
    {
        var employee = await this.context.Employees
            .Where(e => e.IsActive)
            .Include(e => e.Department)
            .FirstOrDefaultAsync(
                e => e.Id == request.Id,
                cancellationToken);

        if (employee is null)
        {
            return null;
        }
        
        employee.Fire();
        await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<FireEmployeeDto>(employee);
    }
}
