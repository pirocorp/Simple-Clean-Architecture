namespace CleanArchitecture.Application.Common.Interfaces;

using Domain.Entities;

public interface IDepartmentService
{
    Task<Department?> GetById(int id);

    Task<IEnumerable<Department>> GetAll();

    Task<Department> CreateDepartment(string name, CancellationToken cancellationToken);

    Task<bool> RenameDepartment(int id, string name, CancellationToken cancellationToken);

    Task<bool> DeleteDepartment(int id, CancellationToken cancellationToken);
}
