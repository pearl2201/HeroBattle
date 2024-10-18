using MasterServer.Domain.Entities.Leaderboard;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class LeaderboardTierConfiguration : IEntityTypeConfiguration<LeaderboardTier>
    {
        public void Configure(EntityTypeBuilder<LeaderboardTier> builder)
        {
            builder.HasKey(e => new { e.LeaderboardDefinitionId, e.Id });
            builder.HasOne(e => e.LeaderboardDefinition).WithMany(e => e.Tiers).HasForeignKey(e => e.LeaderboardDefinitionId);
        }
    }
}
