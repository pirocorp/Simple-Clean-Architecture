namespace CleanArchitecture.Application.Departments.Commands.RenameDepartment;

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using MediatR;

using static CleanArchitecture.Domain.Common.DataConstants.Department;

public record RenameDepartmentCommand : IRequest<RenameDepartmentDetailsDto>
{
    [JsonIgnore]
    public int Id { get; init; }

    [Required]
    [StringLength(NAME_MAX_LENGTH)]
    public required string Name { get; init; }
}
