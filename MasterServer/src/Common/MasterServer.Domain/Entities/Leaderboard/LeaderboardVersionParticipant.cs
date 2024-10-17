namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardVersionParticipant : BaseAuditableEntity
    {
        public int LeaderboardVersionId { get; set; }

        public int LeaderboardDefinitionId { get; set; }

        public LeaderboardVersion LeaderboardVersion { get; set; }
        public long PlayerId { get; set; }
        public Player Player { get; set; }
        public Dictionary<string, object> Metadata { get; set; }
    }
}
