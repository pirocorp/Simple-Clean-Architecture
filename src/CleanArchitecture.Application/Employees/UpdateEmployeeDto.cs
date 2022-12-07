namespace CleanArchitecture.Application.Employees;

using System.ComponentModel.DataAnnotations;

public class UpdateEmployeeDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    public decimal Salary { get; set; }

    public int DepartmentId { get; set; }
}
