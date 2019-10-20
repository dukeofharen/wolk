
using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class AttachmentConfiguration : BaseConfiguration<Attachment>
    {
        protected override void ConfigureEntity(EntityTypeBuilder<Attachment> builder)
        {
            builder.Property(_ => _.Filename)
                .HasMaxLength(300);

            builder.Property(_ => _.MimeType)
                .HasMaxLength(100);

            builder.HasIndex(_ => _.Filename);

            builder.HasOne(_ => _.Note)
                .WithMany(_ => _.Attachments)
                .HasForeignKey(_ => _.NoteId);
        }
    }
}
