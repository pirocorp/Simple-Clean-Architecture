namespace CleanArchitecture.Application.Services;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;

using Microsoft.EntityFrameworkCore;

internal class DepartmentService : IDepartmentService
{
    private readonly IApplicationDbContext context;
    private readonly IDateTimeService dateTimeService;

    public DepartmentService(
        IApplicationDbContext dbContext,
        IDateTimeService dateTimeService)
    {
        this.context = dbContext;
        this.dateTimeService = dateTimeService;
    }

    public async Task<Department?> GetById(int id)
        => await this.context.Departments
            .Where(d => d.IsActive)
            .FirstOrDefaultAsync(d => d.Id == id);

    public async Task<IEnumerable<Department>> GetAll()
        => await this.context.Departments
            .Where(d => d.IsActive)
            .ToListAsync();

    public async Task<Department> CreateDepartment(
        string name, 
        CancellationToken cancellationToken)
    {
        var department = Department.Create(name, this.dateTimeService.Now);

        await this.context.Departments.AddAsync(department, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        return department;
    }

    public async Task<bool> RenameDepartment(
        int id, 
        string name, 
        CancellationToken cancellationToken)
        => await this.UpdateDepartment(
            id, 
            cancellationToken, 
            d => d.Rename(name));

    public async Task<bool> DeleteDepartment(int id, CancellationToken cancellationToken)
        => await UpdateDepartment(
            id, 
            cancellationToken, 
            d => d.Delete());

    private async Task<bool> UpdateDepartment(
        int id,
        CancellationToken cancellationToken,
        Action<Department> action)
    {
        var department = await this.GetById(id);

        if (department is null)
        {
            return false;
        }

        action(department);
        await this.context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
