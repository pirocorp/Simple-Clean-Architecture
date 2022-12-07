namespace CleanArchitecture.Application.Departments;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class DepartmentDetailsDto : IMapFrom<Department>
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = string.Empty;
}
