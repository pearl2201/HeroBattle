using NodaTime;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardVersion : BaseAuditableEntity
    {
        public int VersionId { get; set; }
        public int LeaderboardDefinitionId { get; set; }
        [ForeignKey(nameof(LeaderboardDefinitionId))]
        public LeaderboardDefinition LeaderboardDefinition { get; set; }

        public List<LeaderboardVersionParticipant> Participants { get; set; }

        public Instant StartedAt { get; set; }

        public Instant? EndedAt { get; set; }

        public bool Expired { get; set; }
    }


}
