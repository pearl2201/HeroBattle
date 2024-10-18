using MasterServer.Domain.Entities.Leaderboard;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{

    public class LeaderboardVersionConfiguration : IEntityTypeConfiguration<LeaderboardVersion>
    {
        public void Configure(EntityTypeBuilder<LeaderboardVersion> builder)
        {
            builder.HasKey(e => new {e.LeaderboardDefinitionId, e.VersionId});
            builder.HasOne(e => e.LeaderboardDefinition).WithMany(e => e.Versions).HasForeignKey(e => e.LeaderboardDefinitionId);
        }
    }
}
