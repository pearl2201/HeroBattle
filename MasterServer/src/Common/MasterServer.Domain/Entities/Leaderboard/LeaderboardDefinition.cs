using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Leaderboard
{
    public class LeaderboardDefinition: BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int BucketSize { get; set; }

        public LeaderboardSortOrder SortOrder { get; set; }

        public LeaderboardUpdateType UpdateType { get; set; }



        public LeaderboardResetType ResetType { get; set; }

        public Instant? OnetimeReset { get; set; }

        public string ScheduledReset { get; set; }

        public TieringStrategy TieringStrategy { get; set; }
        public List<LeaderboardTier> Tiers { get; set; }

        public List<LeaderboardVersion> Versions { get; set; }
    }

    public enum LeaderboardSortOrder
    {
        DESC,
        ASC
    }

    public enum LeaderboardUpdateType
    {
        KeepBest,
        KeepLatest,
        Aggregate
    }
    public enum LeaderboardResetType
    {
        ManualReset,
        ScheduledReset,
    }

    public enum TieringStrategy
    {
        Score,
        Rank,
        Percent
    }
}
