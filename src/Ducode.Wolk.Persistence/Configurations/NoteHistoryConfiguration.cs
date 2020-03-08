using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class NoteHistoryConfiguration : BaseConfiguration<NoteHistory>
    {
        public NoteHistoryConfiguration() : base("note_history")
        {
        }


        protected override void ConfigureEntity(EntityTypeBuilder<NoteHistory> builder)
        {
            builder.Property(_ => _.Title)
                .HasMaxLength(200);

            builder.HasIndex(_ => _.Title);

            builder.HasOne(_ => _.Note)
                .WithMany(_ => _.NoteHistory)
                .HasForeignKey(_ => _.NoteId);
        }
    }
}
