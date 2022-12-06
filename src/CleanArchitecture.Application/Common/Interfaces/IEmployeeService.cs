namespace CleanArchitecture.Application.Common.Interfaces;

using CleanArchitecture.Domain.Enums;

using Domain.Entities;

public interface IEmployeeService
{
    Task<Employee?> GetById(int id);

    Task<IEnumerable<Employee>> GetAll();

    Task<Employee> CreateEmployee(
        string name,
        int age,
        string email,
        string address,
        Gender gender,
        decimal salary,
        int departmentId,
        CancellationToken cancellationToken);

    Task<bool> ChangeDepartment(int employeeId, int departmentId, CancellationToken cancellationToken);

    Task<bool> FireEmployee(int employeeId, CancellationToken cancellationToken);

    Task<bool> UpdateFile(int employeeId, string address, string email, CancellationToken cancellationToken);

    Task<bool> UpdateSalary(int id, decimal salary, CancellationToken cancellationToken);
}
