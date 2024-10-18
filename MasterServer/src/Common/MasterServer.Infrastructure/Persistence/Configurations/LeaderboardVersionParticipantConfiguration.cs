using MasterServer.Domain.Entities.Leaderboard;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class LeaderboardVersionParticipantConfiguration : IEntityTypeConfiguration<LeaderboardTier>
    {
        public void Configure(EntityTypeBuilder<LeaderboardTier> builder)
        {
            builder.HasKey(e => new { e.LeaderboardDefinitionId, e.Id });
            builder.HasOne(e => e.LeaderboardDefinition).WithMany(e => e.Tiers).HasForeignKey(e => e.LeaderboardDefinitionId);
        }
    }
}
