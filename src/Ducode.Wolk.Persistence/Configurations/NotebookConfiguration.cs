using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class NotebookConfiguration : BaseConfiguration<Notebook>
    {
        public NotebookConfiguration() : base("notebooks")
        {
        }

        protected override void ConfigureEntity(EntityTypeBuilder<Notebook> builder)
        {
            builder.Property(_ => _.Name)
                .HasMaxLength(200);

            builder.HasIndex(_ => _.Name);
        }
    }
}
