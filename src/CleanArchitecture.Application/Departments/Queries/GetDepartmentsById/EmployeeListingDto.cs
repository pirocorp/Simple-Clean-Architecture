namespace CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class EmployeeListingDto : IMapFrom<Employee>
{
    public int Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public void Mapping(Profile profile)
    {
        profile
            .CreateMap<Employee, EmployeeListingDto>()
            .ForMember(
                d => d.Address,
                opt
                    => opt.MapFrom(s => s.Address.ToString()))
            .ForMember(
                d => d.Gender,
                opt
                    => opt.MapFrom(s => s.Gender.ToString()));
    }
}
