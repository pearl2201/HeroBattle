using NodaTime;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassParticipantMilestone
    {

        public BattlePassMilestone BattlePassMilestone { get; set; }
        public int Milestone { get; set; }

        public Instant? PremiumReceivedAt { get; set; }

        public Instant? FreemiumReceivedAt { get; set; }

        public long PlayerId { get; set; }

        public int SeasonId { get; set; }

        public BattlePassParticipant Participant { get; set; }
    }
}
