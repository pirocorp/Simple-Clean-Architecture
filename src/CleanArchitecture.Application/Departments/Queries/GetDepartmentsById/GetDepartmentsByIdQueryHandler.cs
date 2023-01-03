namespace CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using MediatR;

using Microsoft.EntityFrameworkCore;

public class GetDepartmentsByIdQueryHandler
    : IRequestHandler<GetDepartmentsByIdQuery, DepartmentFullDetailsDto?>
{
    private readonly IDbContext context;
    private readonly IMapper mapper;

    public GetDepartmentsByIdQueryHandler(
        IDbContext context, 
        IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<DepartmentFullDetailsDto?> Handle(
        GetDepartmentsByIdQuery request, 
        CancellationToken cancellationToken)
        => await this.context.Departments
            .AsNoTracking()
            .Where(d => d.IsActive)
            .ProjectTo<DepartmentFullDetailsDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(
                d => d.Id == request.Id, 
                cancellationToken: cancellationToken);
}
