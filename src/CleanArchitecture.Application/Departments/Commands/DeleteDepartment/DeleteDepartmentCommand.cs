namespace CleanArchitecture.Application.Departments.Commands.DeleteDepartment;

using MediatR;

public record DeleteDepartmentCommand(int Id) : IRequest<DeleteDepartmentDetailsDto>;
