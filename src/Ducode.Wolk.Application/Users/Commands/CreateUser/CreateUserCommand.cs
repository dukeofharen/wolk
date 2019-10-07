using MediatR;

namespace Ducode.Wolk.Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest
    {
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
