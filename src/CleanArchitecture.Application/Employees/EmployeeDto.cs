namespace CleanArchitecture.Application.Employees;

using AutoMapper;

using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Application.Common.Mappings;

public class EmployeeDto : IMapFrom<Employee>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public string Department { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile
            .CreateMap<Employee, EmployeeDto>()
            .ForMember(
                d => d.Department,
                opt
                    => opt.MapFrom(s => s.Department.Name));
    }
}
