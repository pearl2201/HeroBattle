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
    public class BattlePassSeasonParticipantConfiguration : IEntityTypeConfiguration<BattlePassSeasonParticipant>
    {
        public void Configure(EntityTypeBuilder<BattlePassSeasonParticipant> builder)
        {
            builder.HasKey(e => new { e.SeasonId, e.PlayerId });
            builder.HasOne(e => e.Player).WithMany(e => e.BattlePassParticipants).HasForeignKey(e => e.PlayerId);
            builder.HasOne(e => e.Season).WithMany(e => e.Participants).HasForeignKey(e => e.SeasonId);
        }
    }
}
