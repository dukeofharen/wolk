using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ducode.Wolk.Application.Interfaces
{
    public interface IWolkDbContext
    {
        DbSet<Note> Notes { get; set; }

        DbSet<Notebook> Notebooks { get; set; }
    }
}
