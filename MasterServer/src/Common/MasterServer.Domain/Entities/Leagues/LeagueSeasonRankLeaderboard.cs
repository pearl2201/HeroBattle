using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeasonRankLeaderboard : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }



        public int SeasonRankId { get; set; }
        [ForeignKey("SeasonRankId")]
        public LeagueSeasonRank SeasonRank { get; set; }

        public List<LeagueSeasonParticipant> Participants { get; set; }
    }
}
