using System.Threading.Tasks;

namespace Ducode.Wolk.Persistence.SaveChanges
{
    public interface ISaveChangesHandler
    {
        Task HandleSaveChangesAsync(WolkDbContext context);
    }
}
