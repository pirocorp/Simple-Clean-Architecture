namespace CleanArchitecture.Application.Departments.Commands.CreateDepartment;

using MediatR;

public record CreateDepartmentCommand : IRequest<CreateDepartmentDetailsDto>
{
    public required string Name { get; init; }
}
