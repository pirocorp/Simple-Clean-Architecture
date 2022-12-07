namespace CleanArchitecture.Application.Departments;

using System.Data;

using CleanArchitecture.Domain.Enums;

public class EmployeeListingDto
{
    public int Id { get; set; }

    public string Address { get; set; } = string.Empty;

    public int Age { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Email { get; set; } = string.Empty;

    public string Gender { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public decimal Salary { get; set; }
}
