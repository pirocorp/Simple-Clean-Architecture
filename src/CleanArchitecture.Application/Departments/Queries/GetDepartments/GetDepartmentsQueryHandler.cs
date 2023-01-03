namespace CleanArchitecture.Application.Departments.Queries.GetDepartments;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;

using MediatR;
using Microsoft.EntityFrameworkCore;

public class GetDepartmentsQueryHandler
    : IRequestHandler<GetDepartmentsQuery, IEnumerable<DepartmentListingDto>>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public GetDepartmentsQueryHandler(
        IDbContext context, 
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<IEnumerable<DepartmentListingDto>> Handle(
        GetDepartmentsQuery request, 
        CancellationToken cancellationToken)
        => await this.context.Departments
            .Where(d => d.IsActive)
            .ProjectTo<DepartmentListingDto>(this.mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken: cancellationToken);
} 
