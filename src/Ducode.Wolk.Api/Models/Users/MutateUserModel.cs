using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Application.Users.Commands.CreateUser;

namespace Ducode.Wolk.Api.Models.Users
{
    /// <summary>
    /// Model used for creating / updating users.
    /// </summary>
    public class MutateUserModel : IMapTo<CreateUserCommand>
    {
        /// <summary>
        /// Gets or sets the user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the user password.
        /// </summary>
        public string Password { get; set; }
    }
}
