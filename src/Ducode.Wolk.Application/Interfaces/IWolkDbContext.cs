using System.Threading;
using System.Threading.Tasks;
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Ducode.Wolk.Application.Interfaces
{
    public interface IWolkDbContext
    {
        DbSet<Attachment> Attachments { get; set; }

        DbSet<Note> Notes { get; set; }

        DbSet<NoteHistory> NoteHistory { get; set; }

        DbSet<Notebook> Notebooks { get; set; }

        DbSet<User> Users { get; set; }

        DbSet<AccessToken> AccessTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

        IDbContextTransaction BeginTransaction();
    }
}
