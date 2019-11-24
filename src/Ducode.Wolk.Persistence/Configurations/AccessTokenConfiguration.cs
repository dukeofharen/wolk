using Ducode.Wolk.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ducode.Wolk.Persistence.Configurations
{
    public class AccessTokenConfiguration : BaseConfiguration<AccessToken>
    {
        public AccessTokenConfiguration() : base("access_tokens")
        {
        }


        protected override void ConfigureEntity(EntityTypeBuilder<AccessToken> builder)
        {
            builder.Property(_ => _.Token)
                .HasMaxLength(300);

            builder.Property(_ => _.Identifier)
                .HasMaxLength(100);

            // Indices
            builder.HasIndex(_ => _.Token)
                .IsUnique();

            builder.HasIndex(_ => _.ExpirationDateTime);

            builder.HasIndex(_ => _.AccessTokenType);

            builder.HasIndex(_ => _.Identifier);
        }
    }
}
