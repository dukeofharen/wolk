using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ducode.Wolk.Api.Controllers
{
    [Route("api/[controller]")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public abstract class BaseApiController : Controller
    {
        private IMapper _mapper;
        private IMediator _mediator;

        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetRequiredService<IMapper>();

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    }
}
