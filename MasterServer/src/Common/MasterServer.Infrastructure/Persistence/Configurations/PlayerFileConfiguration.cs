using MasterServer.Domain.Entities.GameSave;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class PlayerFileConfiguration : IEntityTypeConfiguration<PlayerFile>
    {
        public void Configure(EntityTypeBuilder<PlayerFile> builder)
        {
            builder.HasKey(x => new { x.PlayerId, x.Key });
            builder.HasOne(x => x.Player).WithMany(x => x.PlayerFiles).HasForeignKey(x => x.PlayerId);
        }
    }
}
