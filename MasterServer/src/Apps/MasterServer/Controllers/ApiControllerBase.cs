using MasterServer.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace MasterServer.Controllers
{
    [ApiController]
    [ApiExceptionFilter]
    [Route("api/v{version:int}/[controller]")]
    public abstract class ApiControllerBase : ControllerBase
    {
        private ISender? _mediator;

        protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
    }
}
