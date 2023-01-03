namespace CleanArchitecture.Presentation.Controllers;

using CleanArchitecture.Application.Departments.Commands.CreateDepartment;
using CleanArchitecture.Application.Departments.Commands.DeleteDepartment;
using CleanArchitecture.Application.Departments.Commands.RenameDepartment;
using CleanArchitecture.Application.Departments.Queries.GetDepartments;
using CleanArchitecture.Application.Departments.Queries.GetDepartmentsById;

using Microsoft.AspNetCore.Mvc;

public class DepartmentsController : ApiControllerBase
{
    [HttpGet(WITH_ID)]
    public async Task<ActionResult<DepartmentFullDetailsDto>> GetById(int id)
        => this.OkOrNotFound(await this.Mediator
            .Send(new GetDepartmentsByIdQuery(id)));

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DepartmentListingDto>>> GetDepartments(
        CancellationToken cancellationToken)
            => this.Ok(await this.Mediator
                .Send(new GetDepartmentsQuery(), cancellationToken));

    [HttpPost]
    public async Task<ActionResult<CreateDepartmentDetailsDto>> CreateDepartment(
        CreateDepartmentCommand command,
        CancellationToken cancellationToken)
        => await this.Mediator.Send(command, cancellationToken);

    [HttpPatch(WITH_ID)]
    public async Task<ActionResult<RenameDepartmentDetailsDto>> RenameDepartment(
        int id,
        RenameDepartmentCommand command,
        CancellationToken cancellationToken)
        => this.OkOrNotFound(await this.Mediator
            .Send(command with { Id = id }, cancellationToken));

    [HttpDelete(WITH_ID)]
    public async Task<ActionResult<DeleteDepartmentDetailsDto>> Delete(int id, CancellationToken cancellationToken)
        => this.OkOrNotFound(await this.Mediator
            .Send(new DeleteDepartmentCommand(id), cancellationToken));
}
