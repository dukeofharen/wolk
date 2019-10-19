using System.Threading.Tasks;
using Ducode.Wolk.Application.Users.Commands.CreateUser;
using Ducode.Wolk.Application.Users.Models;
using Ducode.Wolk.Application.Users.Queries.SignIn;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ducode.Wolk.Api.Controllers
{
    public class UserController : BaseApiController
    {
        /// <summary>
        /// Endpoint for registering new user.
        /// </summary>
        /// <param name="command">The user.</param>
        /// <returns>No content.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody]CreateUserCommand command)
        {
            await Mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Endpoint for authenticating a user.
        /// </summary>
        /// <param name="query">The sign in data.</param>
        /// <returns>A model containing the necessary user data.</returns>
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<SignedInViewModel>> Authenticate([FromBody] SignInQuery query)
            => Ok(await Mediator.Send(query));
    }
}
