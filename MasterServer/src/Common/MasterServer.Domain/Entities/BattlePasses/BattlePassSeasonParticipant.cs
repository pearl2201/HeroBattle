using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassSeasonParticipant : BaseAuditableEntity
    {
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        public long PlayerId { get; set; }

        public int Trophy { get; set; }
        public Instant? PremiumActivedAt { get; set; }

        public int SeasonId { get; set; }
        [ForeignKey(nameof(SeasonId))]
        public BattlePassSeason Season { get; set; }

        public List<BattlePassSeasonParticipantMilestone> MilestoneParticipants { get; set; }
    }
}
