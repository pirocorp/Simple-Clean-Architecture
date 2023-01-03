namespace CleanArchitecture.Presentation.Controllers
{
    using CleanArchitecture.Application.Employees.Commands.CreateEmployee;
    using CleanArchitecture.Application.Employees.Commands.FireEmployee;
    using CleanArchitecture.Application.Employees.Commands.UpdateEmployee;
    using CleanArchitecture.Application.Employees.Queries.GetEmployees;
    using CleanArchitecture.Application.Employees.Queries.GetEmployeesById;

    using Microsoft.AspNetCore.Mvc;

    public class EmployeesController : ApiControllerBase
    {
        [HttpGet(WITH_ID)]
        public async Task<ActionResult<GetEmployeesByIdDto>> GetById(
            int id, 
            CancellationToken cancellationToken)
            => this.OkOrNotFound(await this.Mediator
                .Send(new GetEmployeesByIdQuery(id), cancellationToken));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetEmployeesListingDto>>> GetEmployees()
            => this.Ok(await this.Mediator.Send(new GetEmployeesQuery()));

        [HttpPost]
        public async Task<ActionResult<CreateEmployeeDto>> CreateEmployee(
            CreateEmployeeCommand input,
            CancellationToken cancellationToken)
            => await this.Mediator.Send(input, cancellationToken);

        [HttpDelete(WITH_ID)]
        public async Task<ActionResult<FireEmployeeDto>> FireEmployee(
            int id,
            CancellationToken cancellationToken)
            => this.Ok(await this.Mediator
                .Send(new FireEmployeeCommand(id), cancellationToken));

        [HttpPost(WITH_ID)]
        public async Task<ActionResult<UpdateEmployeeDto>> UpdateEmployee(
            int id,
            UpdateEmployeeCommand command,
            CancellationToken cancellationToken)
            => this.OkOrNotFound(await this.Mediator.Send(
                command with { Id = id }, cancellationToken));
    }
}
