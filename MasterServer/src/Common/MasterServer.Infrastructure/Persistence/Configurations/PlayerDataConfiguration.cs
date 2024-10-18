using MasterServer.Domain.Entities.GameSave;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class PlayerDataConfiguration : IEntityTypeConfiguration<PlayerData>
    {
        public void Configure(EntityTypeBuilder<PlayerData> builder)
        {
            builder.HasKey(x => new { x.PlayerId, x.Key });
            builder.HasOne(x => x.Player).WithMany(x => x.PlayerDatas).HasForeignKey(x => x.PlayerId);
        }
    }
}
