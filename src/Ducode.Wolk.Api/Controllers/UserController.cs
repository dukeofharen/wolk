using System.Threading.Tasks;
using Ducode.Wolk.Api.Models.Users;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Users.Commands.CreateUser;
using Ducode.Wolk.Application.Users.Models;
using Ducode.Wolk.Application.Users.Queries.SignIn;
using Ducode.Wolk.Configuration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Ducode.Wolk.Api.Controllers
{
    public class UserController : BaseApiController
    {
        private readonly WolkConfiguration _configuration;

        public UserController(IOptions<WolkConfiguration> options)
        {
            _configuration = options.Value;
        }

        /// <summary>
        /// Endpoint for registering new user.
        /// </summary>
        /// <param name="userModel">The user.</param>
        /// <returns>No content.</returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [AllowAnonymous]
        public async Task<ActionResult> Register([FromBody]MutateUserModel userModel)
        {
            if (!_configuration.EnableUserRegistration)
            {
                throw new ValidationException("User registration not allowed according to condiguration.");
            }

            await Mediator.Send(Mapper.Map<CreateUserCommand>(userModel));
            return NoContent();
        }

        /// <summary>
        /// Endpoint for authenticating a user.
        /// </summary>
        /// <param name="signInModel">The sign in data.</param>
        /// <returns>A model containing the necessary user data.</returns>
        [HttpPost]
        [Route("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<SignedInViewModel>> Authenticate([FromBody] SignInModel signInModel)
            => Ok(await Mediator.Send(Mapper.Map<SignInQuery>(signInModel)));
    }
}
