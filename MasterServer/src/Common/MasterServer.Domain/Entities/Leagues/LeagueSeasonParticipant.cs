using MasterServer.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeasonParticipant : BaseAuditableEntity
    {
        public int? LeaderboardId { get; set; }

        [ForeignKey("LeaderboardId")]
        public LeagueSeasonRankLeaderboard Leaderboard { get; set; }
        public LeagueSeason LeagueSeason { get; set; }
        public int LeagueSeasonId { get; set; }

        public long PlayerId { get; set; }

        public Player Player { get; set; }

        public GameLeagueRank CurrentRank { get; set; }

        public int Star { get; set; }

        public List<LeagueSeasonParticipantRank> RankRecords { get; set; }

        public int SuccessfulAttack { get; set; }

        public int TotalMatch { get; set; }

        public float WinRate => TotalMatch == 0 ? 0 : ((float)SuccessfulAttack) / TotalMatch;

        //public int SuccessfulDefense { get; set; }
    }
}
