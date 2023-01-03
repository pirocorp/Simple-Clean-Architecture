namespace CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;

using MediatR;

public record GetDepartmentsByIdQuery(int Id) : IRequest<DepartmentFullDetailsDto>;