using System.Threading.Tasks;

namespace Ducode.Wolk.Application.Interfaces.Identity
{
    public interface ITokenRenewalManager
    {
        Task<string> RenewTokenAsync(long userId);
    }
}
