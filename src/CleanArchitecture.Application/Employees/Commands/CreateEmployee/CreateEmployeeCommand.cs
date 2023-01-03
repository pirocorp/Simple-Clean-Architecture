namespace CleanArchitecture.Application.Employees.Commands.CreateEmployee;

using System.ComponentModel.DataAnnotations;
using MediatR;

using static CleanArchitecture.Domain.Common.DataConstants.Employee;

public class CreateEmployeeCommand : IRequest<CreateEmployeeDto>
{
    [Required]
    [StringLength(ADDRESS_MAX_LENGTH)]
    public required string Address { get; init; }

    [Range(AGE_MIN_VALUE, AGE_MAX_VALUE)]
    public int Age { get; init; }

    [Required]
    [EmailAddress]
    [StringLength(EMAIL_MAX_LENGTH)]
    public required string Email { get; init; }

    [Required]
    public required string Gender { get; init; }

    [Required]
    [StringLength(NAME_MAX_LENGTH)]
    public required string Name { get; init; }

    [Range(SALARY_MIN_VALUE, SALARY_MAX_VALUE)]
    public decimal Salary { get; init; }

    public int DepartmentId { get; init; }
}
