namespace CleanArchitecture.Application.Employees;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class EmployeeDetailsDto : IMapFrom<Employee>
{
    public int Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }

    public string Department { get; set; } = string.Empty;

    public void Mapping(Profile profile)
    {
        profile
            .CreateMap<Employee, EmployeeDetailsDto>()
            .ForMember(
                d => d.Address,
                opt
                    => opt.MapFrom(s => s.Address.ToString()))
            .ForMember(
                d => d.Gender,
                opt
                    => opt.MapFrom(s => s.Gender.ToString()))
            .ForMember(
                d => d.Department,
                opt
                    => opt.MapFrom(s => s.Department.Name));
    }
}
