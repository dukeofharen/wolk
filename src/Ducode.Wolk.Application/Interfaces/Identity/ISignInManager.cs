using System.Threading.Tasks;

namespace Ducode.Wolk.Application.Interfaces.Identity
{
    public interface ISignInManager
    {
        Task<bool> CheckCredentialsAsync(string email, string password);
    }
}
