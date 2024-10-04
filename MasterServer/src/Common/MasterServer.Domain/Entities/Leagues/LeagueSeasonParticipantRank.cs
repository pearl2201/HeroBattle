using MasterServer.Domain.Enums;
using NodaTime;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeasonParticipantRank : BaseAuditableEntity
    {
        public int LeagueSeasonId { get; set; }

        public long PlayerId { get; set; }

        public LeagueSeasonParticipant Participant { get; set; }

        public GameLeagueRank Rank { get; set; }

        public Instant? ReceivedAt { get; set; }
    }
}
