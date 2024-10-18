using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardTier : BaseAuditableEntity
    {
        public int LeaderboardDefinitionId { get; set; }
        [ForeignKey(nameof(LeaderboardDefinitionId))]
        public LeaderboardDefinition LeaderboardDefinition { get; set; }
        public int Id { get; set; }

        public string Name { get; set; }

        public float Cutoff { get; set; }
    }
}
