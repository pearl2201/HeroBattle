namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardTier : BaseAuditableEntity
    {
        public int Id { get; set; }

        public float Cutoff { get; set; }
    }
}
