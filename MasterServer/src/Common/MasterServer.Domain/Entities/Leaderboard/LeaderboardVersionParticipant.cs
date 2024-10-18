using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardVersionParticipant : BaseAuditableEntity
    {
        public int VersionParticipantId { get; set; } // use for bucket leaderboard // we have to use trigger at this
        public int LeaderboardVersionId { get; set; }

        public int LeaderboardDefinitionId { get; set; }

        public LeaderboardVersion LeaderboardVersion { get; set; }
        public long PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        
        public LeaderboardTier CurrentTier { get; set; }

        public int CurrentTierId { get; set; }

        public List<LeaderboardVersionParticipantTier> VerionTiers { get; set; }

        
    }
}
