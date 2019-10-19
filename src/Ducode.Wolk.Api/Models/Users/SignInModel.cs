using Ducode.Wolk.Application.Interfaces.Mappings;

namespace Ducode.Wolk.Api.Models.Users
{
    /// <summary>
    /// A model used for authenticating a user.
    /// </summary>
    public class SignInModel : IMapTo<SignInModel>
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
