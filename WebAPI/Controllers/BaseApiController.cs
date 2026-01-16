using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class BaseApiController : ControllerBase
    {
        //esta propiedad se usa para acceder a MediatR desde los controladores que heredan de BaseApiController
        private IMediator? _mediator;

        //esta es una propiedad protegida que devuelve una instancia de IMediator
        protected IMediator? Mediator => _mediator ?? HttpContext.RequestServices.GetService<IMediator>();
    }
}
