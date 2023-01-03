namespace CleanArchitecture.Application.Employees.Queries.GetEmployees;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetEmployeesHandler 
    : IRequestHandler<GetEmployeesQuery, IEnumerable<GetEmployeesListingDto>>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public GetEmployeesHandler(IDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<GetEmployeesListingDto>> Handle(
        GetEmployeesQuery request, 
        CancellationToken cancellationToken)
        => await this.context.Employees
            .Where(e => e.IsActive)
            .ProjectTo<GetEmployeesListingDto>(this.mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
}
