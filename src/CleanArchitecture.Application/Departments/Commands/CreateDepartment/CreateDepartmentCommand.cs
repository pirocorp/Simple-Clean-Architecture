namespace CleanArchitecture.Application.Departments.Commands.CreateDepartment;

using MediatR;

// TODO: Add Validations 
public record CreateDepartmentCommand : IRequest<CreateDepartmentDetailsDto>
{
    public required string Name { get; init; }
}
