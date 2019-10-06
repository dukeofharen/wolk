using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Ducode.Wolk.Api.Controllers
{
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ApiController]
    public abstract class BaseApiController
    {
        private IMapper _mapper;
        private IMediator _mediator;

        protected IMapper Mapper => _mapper ?? (_mapper = HttpContext.RequestServices.GetRequiredService<IMapper>());

        protected IMediator Mediator =>
            _mediator ?? (_mediator = HttpContext.RequestServices.GetRequiredService<IMediator>());
    }
}
