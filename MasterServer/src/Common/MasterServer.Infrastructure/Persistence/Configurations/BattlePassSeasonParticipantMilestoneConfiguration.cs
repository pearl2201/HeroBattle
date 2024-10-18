using MasterServer.Domain.Entities.BattlePasses;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Infrastructure.Persistence.Configurations
{
    public class BattlePassSeasonParticipantMilestoneConfiguration : IEntityTypeConfiguration<BattlePassSeasonParticipantMilestone>
    {
        public void Configure(EntityTypeBuilder<BattlePassSeasonParticipantMilestone> builder)
        {
            builder.HasKey(e => new { e.SeasonId, e.MilestoneId, e.PlayerId });
            builder.HasOne(e => e.BattlePassMilestone).WithMany(e => e.MilestoneParticipants).HasForeignKey(e => new { e.SeasonId, e.MilestoneId });
            builder.HasOne(e => e.Participant).WithMany(e => e.MilestoneParticipants).HasForeignKey(e => new { e.SeasonId, e.PlayerId });
        }
    }
}
