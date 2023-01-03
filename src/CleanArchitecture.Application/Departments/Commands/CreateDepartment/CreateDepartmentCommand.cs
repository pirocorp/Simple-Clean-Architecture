namespace CleanArchitecture.Application.Departments.Commands.CreateDepartment;

using System.ComponentModel.DataAnnotations;
using MediatR;

using static CleanArchitecture.Domain.Common.DataConstants.Department;

public record CreateDepartmentCommand : IRequest<CreateDepartmentDetailsDto>
{
    [Required]
    [StringLength(NAME_MAX_LENGTH)]
    public required string Name { get; init; }
}
