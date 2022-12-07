namespace CleanArchitecture.Application.Departments;

using CleanArchitecture.Domain.Entities;

public class DepartmentFullDetailsDto : DepartmentDetailsDto
{
    public DepartmentFullDetailsDto()
    {
        this.Employees = new List<EmployeeListingDto>();
    }

    public IEnumerable<EmployeeListingDto> Employees { get; set; }
}
