using MediatR;

namespace Ducode.Wolk.Application.Users.Queries.UserValid
{
    public class UserValidQuery : IRequest<bool>
    {
        public long UserId { get; set; }

        public string SecurityStamp { get; set; }
    }
}
