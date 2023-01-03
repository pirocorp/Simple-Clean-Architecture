namespace CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class DepartmentFullDetailsDto : IMapFrom<Department>
{
    public DepartmentFullDetailsDto()
    {
        this.Employees = new List<EmployeeListingDto>();
    }

    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = string.Empty;

    public IEnumerable<EmployeeListingDto> Employees { get; set; }

    public void Mapping(Profile profile)
        => profile
            .CreateMap<Department, DepartmentFullDetailsDto>()
            .ForMember(
                d => d.Employees,
                opt
                    => opt.MapFrom(s => s.Employees));
}
