namespace CleanArchitecture.Application.Services;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;

using Microsoft.EntityFrameworkCore;

internal class EmployeeService : IEmployeeService
{
    private readonly IApplicationDbContext context;
    private readonly IDateTimeService dateTimeServiceService;
    private readonly IDepartmentService departmentService;

    public EmployeeService(
        IApplicationDbContext dbContext,
        IDateTimeService dateTimeServiceService,
        IDepartmentService departmentService)
    {
        this.context = dbContext;
        this.dateTimeServiceService = dateTimeServiceService;
        this.departmentService = departmentService;
    }

    public async Task<Employee?> GetById(int id)
        => await this.context.Employees
            .Where(e => e.IsActive)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Employee>> GetAll()
        => await this.context.Employees
            .Where(e => e.IsActive)
            .ToListAsync();

    public async Task<Employee> CreateEmployee(
        string name,
        int age,
        string email,
        string address,
        Gender gender,
        decimal salary,
        int departmentId,
        CancellationToken cancellationToken)
    {
        var employee = Employee.Create(
            name, 
            age, 
            this.dateTimeServiceService.Now,
            email, 
            address, 
            gender, 
            salary, departmentId);

        await this.context.Employees.AddAsync(employee, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        return employee;
    }

    public async Task<bool> ChangeDepartment(
        int employeeId, 
        int departmentId,
        CancellationToken cancellationToken)
    {
        var department = await this.departmentService.GetById(departmentId);

        if (department is null)
        {
            return false;
        }

        return await this.UpdateEmployee(
            employeeId,
            cancellationToken,
            e => e.ChangeDepartment(departmentId));
    }

    public async Task<bool> FireEmployee(
        int employeeId, 
        CancellationToken cancellationToken)
        => await this.UpdateEmployee(
            employeeId, 
            cancellationToken, 
            e => e.Fire());

    public async Task<bool> UpdateFile(
        int employeeId,
        string address,
        string email,
        CancellationToken cancellationToken)
        => await this.UpdateEmployee(
            employeeId,
            cancellationToken,
            e => e.UpdateFile(address, email));

    public async Task<bool> UpdateSalary(
        int id,
        decimal salary,
        CancellationToken cancellationToken)
        => await this.UpdateEmployee(
            id,
            cancellationToken,
            e => e.UpdateSalary(salary));

    private async Task<bool> UpdateEmployee(
        int employeeId, 
        CancellationToken cancellationToken,
        Action<Employee> action)
    {
        var employee = await this.GetById(employeeId);

        if (employee is null)
        {
            return false;
        }

        action(employee);
        await this.context.SaveChangesAsync(cancellationToken);

        return true;
    }
}
