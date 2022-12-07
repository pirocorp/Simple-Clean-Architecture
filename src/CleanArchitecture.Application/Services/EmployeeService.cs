namespace CleanArchitecture.Application.Services;

using AutoMapper;
using AutoMapper.QueryableExtensions;

using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

internal class EmployeeService : IEmployeeService
{
    private readonly IDbContext context;
    private readonly IDateTimeService dateTimeServiceService;
    private readonly IDepartmentService departmentService;
    private readonly IMapper mapper;

    public EmployeeService(
        IDbContext dbContext,
        IDateTimeService dateTimeServiceService,
        IDepartmentService departmentService,
        IMapper mapper)
    {
        this.context = dbContext;
        this.dateTimeServiceService = dateTimeServiceService;
        this.departmentService = departmentService;
        this.mapper = mapper;
    }

    public async Task<EmployeeDetailsDto?> GetById(int id)
        => await this.context.Employees
            .Where(e => e.IsActive)
            .AsNoTracking()
            .ProjectTo<EmployeeDetailsDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<EmployeeDto>> GetAll()
        => await this.context.Employees
            .Where(e => e.IsActive)
            .ProjectTo<EmployeeDto>(this.mapper.ConfigurationProvider)
            .ToListAsync();

    public async Task<EmployeeDetailsDto> CreateEmployee(
        CreateEmployeeDto input,
        CancellationToken cancellationToken)
    {
        var valid = Enum.TryParse<Gender>(input.Gender, true, out var gender);

        if (!valid)
        {
            throw new InvalidGenderException();
        }

        var employee = Employee.Create(
            input.Name, 
            input.Age, 
            this.dateTimeServiceService.Now,
            input.Email, 
            input.Address, 
            gender, 
            input.Salary, 
            input.DepartmentId);

        await this.context.Employees.AddAsync(employee, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        return (await this.GetById(employee.Id))!;
    }

    public async Task<EmployeeDto?> FireEmployee(
        int employeeId, 
        CancellationToken cancellationToken)
        => await this.UpdateEmployee<EmployeeDto>(
            employeeId, 
            cancellationToken, 
            e => e.Fire());

    public async Task<EmployeeDetailsDto?> Update(
        int id,
        UpdateEmployeeDto input,
        CancellationToken cancellationToken)
    {
        var employee = await this.ById(id);

        if (employee is null)
        {
            return null;
        }

        employee.Name = input.Name;
        employee.Age = input.Age;
        employee.Email = input.Email;
        employee.Address = Address.From(input.Address);
        employee.Salary = input.Salary;
        employee.DepartmentId = input.DepartmentId;

        await this.context.SaveChangesAsync(cancellationToken);

        return await this.GetById(employee.Id);
    }

    private async Task<Employee?> ById(int id)
        => await this.context.Employees
            .Where(e => e.IsActive)
            .FirstOrDefaultAsync(e => e.Id == id);

    private async Task<T?> UpdateEmployee<T>(
        int employeeId, 
        CancellationToken cancellationToken,
        Action<Employee> action)
    {
        var employee = await this.ById(employeeId);

        if (employee is null)
        {
            return default(T);
        }

        action(employee);
        await this.context.SaveChangesAsync(cancellationToken);

        return this.mapper.Map<T>(employee);
    }
}
