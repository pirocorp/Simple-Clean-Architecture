namespace CleanArchitecture.Application.Services;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Departments;
using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;

internal class DepartmentService : IDepartmentService
{
    private readonly IDbContext context;
    private readonly IDateTimeService dateTimeService;

    public DepartmentService(
        IDbContext dbContext,
        IDateTimeService dateTimeService)
    {
        this.context = dbContext;
        this.dateTimeService = dateTimeService;
    }

    public async Task<DepartmentFullDetailsDto?> GetById(int id)
        => await this.context.Departments
            .AsNoTracking()
            .Where(d => d.IsActive)
            .Select(d => new DepartmentFullDetailsDto
            {
                Id = d.Id,
                Name = d.Name,
                CreatedAt = d.CreatedAt,
                Employees = d.Employees.Select(e => new EmployeeListingDto()
                {
                    Id = e.Id,
                    Address = e.Address.ToString(),
                    Age = e.Age,
                    CreatedAt = e.CreatedAt,
                    Email = e.Email,
                    Gender = e.Gender.ToString(),
                    Name = e.Name,
                    Salary = e.Salary,
                })
            })
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<DepartmentListingDto>> GetAll()
        => await this.context.Departments
            .Where(d => d.IsActive)
            .Select(d => new DepartmentListingDto
            {
                Id = d.Id,
                Name = d.Name,
            })
            .ToListAsync();

    public async Task<DepartmentDetailsDto> CreateDepartment(
        CreateDepartmentDto input, 
        CancellationToken cancellationToken)
    {
        var department = Department.Create(input.Name, this.dateTimeService.Now);

        await this.context.Departments.AddAsync(department, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        var response = new DepartmentDetailsDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt
        };

        return response;
    }

    public async Task<DepartmentDetailsDto?> RenameDepartment(
        int id, 
        RenameDepartmentDto input, 
        CancellationToken cancellationToken)
        => await this.UpdateDepartment(
            id, 
            cancellationToken, 
            d => d.Rename(input.Name));

    public async Task<DepartmentDetailsDto?> DeleteDepartment(int id, CancellationToken cancellationToken)
        => await UpdateDepartment(
            id, 
            cancellationToken, 
            d => d.Delete());

    private async Task<Department?> ById(int id)
        => await this.context.Departments
            .Where(d => d.IsActive)
            .FirstOrDefaultAsync(d => d.Id == id);

    private async Task<DepartmentDetailsDto?> UpdateDepartment(
        int id,
        CancellationToken cancellationToken,
        Action<Department> action)
    {
        var department = await this.ById(id);

        if (department is null)
        {
            return null;
        }

        action(department);
        await this.context.SaveChangesAsync(cancellationToken);

        return new DepartmentDetailsDto
        {
            Id = department.Id,
            Name = department.Name,
            CreatedAt = department.CreatedAt,
        };
    }
}
