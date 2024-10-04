using NodaTime;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassParticipant
    {
        public Player Player { get; set; }
        public long PlayerId { get; set; }

        public int Trophy { get; set; }
        public Instant? PremiumActivedAt { get; set; }

        public int SeasonId { get; set; }

        public BattlePassSeason Season { get; set; }

        public List<BattlePassParticipantMilestone> MilestoneParticipants { get; set; }
    }
}
