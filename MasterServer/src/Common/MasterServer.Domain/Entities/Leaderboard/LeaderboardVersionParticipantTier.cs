namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardVersionParticipantTier : BaseAuditableEntity
    {
        public int LeaderboardVersionId { get; set; }

        public int LeaderboardDefinitionId { get; set; }

        public long PlayerId { get; set; }

        public int TierId { get; set; }
    }
}
