using MasterServer.Domain.Entities.Mails;
using MasterServer.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeasonRank : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public GameLeagueRank Rank { get; set; }

        public int MinStar { get; set; }

        public int? MaxStar { get; set; }

        public int SeasonId { get; set; }
        [ForeignKey("SeasonId")]
        public LeagueSeason Season { get; set; }

        public List<LeagueSeasonRankReward> RankRewards { get; set; }

        public List<LeagueSeasonRankLeaderboard> Leaderboards { get; set; }

        [ForeignKey("RewardMailId")]
        public GameMail RewardMail { get; set; }

        public int RewardMailId { get; set; }
    }
}
