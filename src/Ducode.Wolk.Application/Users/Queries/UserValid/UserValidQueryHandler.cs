using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Users.Queries.UserValid
{
    public class UserValidQueryHandler : IRequestHandler<UserValidQuery, bool>
    {
        private readonly IWolkDbContext _wolkDbContext;

        public UserValidQueryHandler(IWolkDbContext wolkDbContext)
        {
            _wolkDbContext = wolkDbContext;
        }

        public async Task<bool> Handle(UserValidQuery request, CancellationToken cancellationToken)
        {
            var user = await _wolkDbContext.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            if (user == null)
            {
                return false;
            }

            if (user.SecurityStamp != request.SecurityStamp)
            {
                return false;
            }

            return true;
        }
    }
}
