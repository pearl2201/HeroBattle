using MasterServer.Domain.Entities.Game;
using MasterServer.Domain.Entities.Economy;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeasonRankReward : BaseAuditableEntity, IRewardable
    {
        public int SeasonRankId { get; set; }
        [ForeignKey("SeasonRankId")]
        public LeagueSeasonRank SeasonRank { get; set; }

        public int SiblingIndex { get; set; }
        [ForeignKey("ItemId")]
        public BaseEconomyDefinition Item { get; set; }

        public string ItemId { get; set; }

        public int Amount { get; set; }
    }
}
