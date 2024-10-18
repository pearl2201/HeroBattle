using MasterServer.Domain.Entities.GameSave;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class GameDataConfiguration : IEntityTypeConfiguration<GameData>
    {
        public void Configure(EntityTypeBuilder<GameData> builder)
        {
            builder.HasKey(x => new { x.Key });
        }
    }
}
