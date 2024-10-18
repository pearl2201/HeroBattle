using MasterServer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder.HasIndex(e => e.UserName).AreNullsDistinct(true);
            builder.HasIndex(e => e.GoogleId).AreNullsDistinct(true);
            builder.HasIndex(e => e.FacebookId).AreNullsDistinct(true);
            builder.HasIndex(e => e.DeviceId).AreNullsDistinct(true);
            builder.HasIndex(e => e.Initialized);
            builder.HasIndex(e => e.Role);
        }
    }
}
