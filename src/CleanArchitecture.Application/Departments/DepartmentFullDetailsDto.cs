namespace CleanArchitecture.Application.Departments;

using AutoMapper;

using CleanArchitecture.Application.Common.Mappings;
using CleanArchitecture.Domain.Entities;

public class DepartmentFullDetailsDto : DepartmentDetailsDto, IMapFrom<Department>
{
    public DepartmentFullDetailsDto()
    {
        this.Employees = new List<EmployeeListingDto>();
    }

    public IEnumerable<EmployeeListingDto> Employees { get; set; }

    public void Mapping(Profile profile)
        => profile
            .CreateMap<Department, DepartmentFullDetailsDto>()
            .ForMember(
                d => d.Employees, 
                opt 
                    => opt.MapFrom(s => s.Employees));
}
