namespace CleanArchitecture.Application.Employees.Queries.GetEmployeesById;

using MediatR;

public record GetEmployeesByIdQuery(int Id) : IRequest<GetEmployeesByIdDto>;
