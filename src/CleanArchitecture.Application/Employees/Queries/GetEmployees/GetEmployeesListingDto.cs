namespace CleanArchitecture.Application.Employees.Queries.GetEmployees;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class GetEmployeesListingDto : IMapFrom<Employee>
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public int DepartmentId { get; set; }

    public string Department { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile
            .CreateMap<Employee, GetEmployeesListingDto>()
            .ForMember(
                d => d.Department,
                opt
                    => opt.MapFrom(s => s.Department.Name));
    }
}
