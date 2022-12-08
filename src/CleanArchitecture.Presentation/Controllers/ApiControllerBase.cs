namespace CleanArchitecture.Presentation.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
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
