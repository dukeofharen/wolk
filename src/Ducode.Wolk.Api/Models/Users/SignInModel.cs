using Ducode.Wolk.Application.Interfaces.Mappings;
using Ducode.Wolk.Application.Users.Queries.SignIn;

namespace Ducode.Wolk.Api.Models.Users
{
    /// <summary>
    /// A model used for authenticating a user.
    /// </summary>
    public class SignInModel : IMapTo<SignInQuery>
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
