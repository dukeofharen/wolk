using System.Threading.Tasks;
using Ducode.Wolk.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Ducode.Wolk.Identity.Interfaces
{
    internal interface IUserManager
    {
        Task<IdentityResult> CreateAsync(User user, string password);
    }
}
