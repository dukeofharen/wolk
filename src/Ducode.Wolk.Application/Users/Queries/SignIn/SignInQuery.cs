using Ducode.Wolk.Application.Users.Models;
using MediatR;

namespace Ducode.Wolk.Application.Users.Queries.SignIn
{
    public class SignInQuery : IRequest<SignedInViewModel>
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
