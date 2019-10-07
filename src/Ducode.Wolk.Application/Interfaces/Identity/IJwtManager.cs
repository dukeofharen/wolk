using Ducode.Wolk.Domain.Entities;

namespace Ducode.Wolk.Application.Interfaces.Identity
{
    public interface IJwtManager
    {
        string CreateJwt(User user);
    }
}
