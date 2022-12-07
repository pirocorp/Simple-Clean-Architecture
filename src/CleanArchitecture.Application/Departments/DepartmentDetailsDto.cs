namespace CleanArchitecture.Application.Departments;

public class DepartmentDetailsDto
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string Name { get; set; } = string.Empty;
}
