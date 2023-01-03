namespace CleanArchitecture.Application.Employees.Commands.CreateEmployee;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Enums;
using CleanArchitecture.Domain.Exceptions;
using MediatR;
using Microsoft.EntityFrameworkCore;

public class CreateEmployeeHandler
    : IRequestHandler<CreateEmployeeCommand, CreateEmployeeDto>
{
    private readonly IDbContext context;
    private readonly IDateTimeService dateTimeServiceService;
    private readonly IMapper mapper;

    public CreateEmployeeHandler(
        IDbContext context, 
        IDateTimeService dateTimeServiceService, 
        IMapper mapper)
    {
        this.context = context;
        this.dateTimeServiceService = dateTimeServiceService;
        this.mapper = mapper;
    }

    public async Task<CreateEmployeeDto> Handle(
        CreateEmployeeCommand request, 
        CancellationToken cancellationToken)
    {
        var valid = Enum
            .TryParse<Gender>(request.Gender, true, out var gender);

        if (!valid)
        {
            throw new InvalidGenderException(nameof(request.Gender));
        }

        var employee = Employee.Create(
            request.Name,
            request.Age,
            this.dateTimeServiceService.Now,
            request.Email,
            request.Address,
            gender,
            request.Salary,
            request.DepartmentId);

        await this.context.Employees.AddAsync(employee, cancellationToken);
        await this.context.SaveChangesAsync(cancellationToken);

        return (await this.context.Employees
            .ProjectTo<CreateEmployeeDto>(this.mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(
                x => x.Id == employee.Id, 
                cancellationToken))!;
    }
}
