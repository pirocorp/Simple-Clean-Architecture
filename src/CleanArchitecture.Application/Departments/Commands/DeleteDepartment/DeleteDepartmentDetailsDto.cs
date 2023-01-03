namespace CleanArchitecture.Application.Departments.Commands.DeleteDepartment;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class DeleteDepartmentDetailsDto : IMapFrom<Department>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
