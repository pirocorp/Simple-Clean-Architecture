namespace CleanArchitecture.Presentation.Controllers;

using MediatR;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    protected const string WITH_ID = "{id:int}";

    private IMediator? mediator;

    protected IMediator Mediator => this.mediator ??= this
        .HttpContext
        .RequestServices
        .GetRequiredService<IMediator>();

    protected ActionResult<T> OkOrNotFound<T>(T? item) where T : class
    {
        if (item is null)
        {
            return this.NotFound();
        }

        return item;
    }

    protected ActionResult NoContentOrNotFound<T>(T? item) where T : class
    {
        if (item is null)
        {
            return this.NotFound();
        }

        return this.NoContent();
    }
}
