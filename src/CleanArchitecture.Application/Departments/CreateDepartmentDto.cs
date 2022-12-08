namespace CleanArchitecture.Application.Departments;

using System.ComponentModel.DataAnnotations;

public class CreateDepartmentDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; } = string.Empty;
}
