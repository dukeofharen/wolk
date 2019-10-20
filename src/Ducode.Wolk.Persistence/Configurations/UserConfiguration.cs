using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class UserConfiguration : BaseConfiguration<User>
    {
        public UserConfiguration() : base("users")
        {
        }

        protected override void ConfigureEntity(EntityTypeBuilder<User> builder)
        {
            builder.Property(_ => _.Email)
                .HasMaxLength(256);

            builder.Property(_ => _.NormalizedEmail)
                .HasMaxLength(256);

            builder.Property(_ => _.PasswordHash)
                .HasMaxLength(256);

            builder.Property(_ => _.SecurityStamp)
                .HasMaxLength(256);

            builder.HasIndex(_ => _.Email)
                .IsUnique();
        }
    }
}
