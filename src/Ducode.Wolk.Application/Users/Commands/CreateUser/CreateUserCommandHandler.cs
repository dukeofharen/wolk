using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces.Identity;
using MediatR;

namespace Ducode.Wolk.Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
    {
        private readonly IRegistrationManager _registrationManager;

        public CreateUserCommandHandler(IRegistrationManager registrationManager)
        {
            _registrationManager = registrationManager;
        }

        public async Task<Unit> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            await _registrationManager.RegisterUserAsync(request.Email, request.Password);
            return Unit.Value;
        }
    }
}
