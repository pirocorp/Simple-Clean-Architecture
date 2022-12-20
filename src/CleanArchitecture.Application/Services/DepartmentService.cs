namespace CleanArchitecture.Application.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments;
using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;

internal class DepartmentService : IDepartmentService
{
    private readonly IDbContext context;
    private readonly IDateTimeService dateTimeService;
    private readonly IMapper mapper;

    public DepartmentService(
        IDbContext dbContext,
        IDateTimeService dateTimeService,
        IMapper mapper)
    {
        this.context = dbContext;
        this.dateTimeService = dateTimeService;
        this.mapper = mapper;
    }

    public async Task<DepartmentFullDetailsDto?> GetById(int id)
        => await this.context.Departments
            .AsNoTracking()
            .Where(d => d.IsActive)
            .ProjectTo<DepartmentFullDetailsDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DepartmentListingDto>> GetAll()
        => await this.context.Departments
            .Where(d => d.IsActive)
            .ProjectTo<DepartmentListingDto>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<DepartmentDetailsDto> CreateDepartment(
        CreateDepartmentDto input, 
        CancellationToken cancellationToken)
    {
        var department = Department.Create(input.Name, this.dateTimeService.Now);

        await this.context.Departments.AddAsync(department, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<DepartmentDetailsDto>(department);
    }

    public async Task<DepartmentDetailsDto?> RenameDepartment(
        int id, 
        RenameDepartmentDto input, 
        CancellationToken cancellationToken)
        => await this.UpdateDepartment<DepartmentDetailsDto>(
            id, 
            cancellationToken, 
            d => d.Rename(input.Name));

    public async Task<DepartmentDetailsDto?> DeleteDepartment(int id, CancellationToken cancellationToken)
        => await UpdateDepartment<DepartmentDetailsDto>(
            id, 
            cancellationToken, 
            d => d.Delete());

    private async Task<Department?> ById(int id)
        => await this.context.Departments
            .Where(d => d.IsActive)
            .FirstOrDefaultAsync(d => d.Id == id);

    private async Task<T?> UpdateDepartment<T>(
        int id,
        CancellationToken cancellationToken,
        Action<Department> action)
    {
        var department = await this.ById(id);

        if (department is null)
        {
            return default(T);
        }

        action(department);
        await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<T>(department);
    }
}
