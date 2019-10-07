using System.Threading.Tasks;

namespace Ducode.Wolk.Application.Interfaces.Identity
{
    public interface IRegistrationManager
    {
        Task RegisterUserAsync(string email, string password);
    }
}
