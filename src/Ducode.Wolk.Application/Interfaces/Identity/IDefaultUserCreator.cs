using System.Threading.Tasks;

namespace Ducode.Wolk.Application.Interfaces.Identity
{
    public interface IDefaultUserCreator
    {
        Task CreateOrUpdateDefaultUser();
    }
}
