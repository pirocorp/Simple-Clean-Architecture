namespace CleanArchitecture.Application.Employees.Commands.FireEmployee;

using MediatR;

public record FireEmployeeCommand(int Id) : IRequest<FireEmployeeDto>;
