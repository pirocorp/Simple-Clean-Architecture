namespace CleanArchitecture.Application.Employees.Commands.CreateEmployee;

using MediatR;

public class CreateEmployeeCommand : IRequest<CreateEmployeeDto>
{
    public required string Address { get; init; }

    public int Age { get; init; }

    public required string Email { get; init; }

    public required string Gender { get; init; }

    public required string Name { get; init; }

    public decimal Salary { get; init; }

    public int DepartmentId { get; init; }
}
