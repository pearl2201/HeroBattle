using MasterServer.Domain.Entities.BattlePasses;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class BattlePassLevelConfiguration : IEntityTypeConfiguration<BattlePassMilestone>
    {
        public void Configure(EntityTypeBuilder<BattlePassMilestone> builder)
        {
            builder.HasKey(e => new { e.SeasonId, e.MilestoneId });
            builder.HasOne(e => e.Season).WithMany(e => e.Milestones).HasForeignKey(e => e.SeasonId);
            builder.HasOne(e => e.Item).WithMany().HasForeignKey(e => e.ItemId);
        }
    }
}
