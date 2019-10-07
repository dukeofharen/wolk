using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class NoteConfiguration : BaseConfiguration<Note>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Note> builder)
        {
            builder.Property(_ => _.Title)
                .HasMaxLength(200);

            builder.HasIndex(_ => _.Title);

            builder.HasOne(n => n.Notebook)
                .WithMany(nb => nb.Notes)
                .HasForeignKey(n => n.NotebookId);
        }
    }
}