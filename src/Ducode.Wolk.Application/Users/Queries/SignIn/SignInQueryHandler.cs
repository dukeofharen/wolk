using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Exceptions;
using Ducode.Wolk.Application.Interfaces;
using Ducode.Wolk.Application.Interfaces.Identity;
using Ducode.Wolk.Application.Users.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Users.Queries.SignIn
{
    public class SignInQueryHandler : IRequestHandler<SignInQuery, SignedInViewModel>
    {
        private readonly IJwtManager _jwtManager;
        private readonly ISignInManager _signInManager;
        private readonly IWolkDbContext _context;

        public SignInQueryHandler(
            IJwtManager jwtManager,
            ISignInManager signInManager,
            IWolkDbContext wolkDbContext)
        {
            _jwtManager = jwtManager;
            _signInManager = signInManager;
            _context = wolkDbContext;
        }

        public async Task<SignedInViewModel> Handle(SignInQuery request, CancellationToken cancellationToken)
        {
            if (!await _signInManager.CheckCredentialsAsync(request.Email, request.Password))
            {
                throw new UnauthorizedException();
            }

            var user = await _context.Users.SingleAsync(u => u.Email == request.Email, cancellationToken);
            var jwt = _jwtManager.CreateJwt(user);
            return new SignedInViewModel
            {
                Email = request.Email,
                Id = user.Id,
                Token = jwt
            };
        }
    }
}
