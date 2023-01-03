namespace CleanArchitecture.Application.Departments.Commands.RenameDepartment;

using System.Text.Json.Serialization;

using MediatR;

public record RenameDepartmentCommand : IRequest<RenameDepartmentDetailsDto>
{
    [JsonIgnore]
    public int Id { get; init; }

    public required string Name { get; init; }
}
