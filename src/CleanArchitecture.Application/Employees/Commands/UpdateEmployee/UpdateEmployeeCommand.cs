namespace CleanArchitecture.Application.Employees.Commands.UpdateEmployee;

using System.Text.Json.Serialization;
using MediatR;

public record UpdateEmployeeCommand : IRequest<UpdateEmployeeDto>
{
    [JsonIgnore]
    public int Id { get; init; }

    public required string Address { get; init; }

    public int Age { get; init; }

    public required string Email { get; init; }

    public required string Name { get; init; }

    public decimal Salary { get; init; }

    public int DepartmentId { get; init; }
}
