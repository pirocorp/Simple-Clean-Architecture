namespace CleanArchitecture.Application.Employees.Queries.GetEmployeesById;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;

using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetEmployeesByIdHandler
    : IRequestHandler<GetEmployeesByIdQuery, GetEmployeesByIdDto?>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public GetEmployeesByIdHandler(IDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<GetEmployeesByIdDto?> Handle(
        GetEmployeesByIdQuery request, 
        CancellationToken cancellationToken)
        => await this.context.Employees
            .Where(e => e.IsActive)
            .AsNoTracking()
            .ProjectTo<GetEmployeesByIdDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(
                e => e.Id == request.Id, 
                cancellationToken);
}
