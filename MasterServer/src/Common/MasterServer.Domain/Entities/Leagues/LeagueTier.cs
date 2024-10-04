using MasterServer.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueTier
    {
        [Key]
        public GameLeagueTier TierId { get; set; }

        public string Name { get; set; }

        public List<GameLeagueRank> Ranks { get; set; }
    }
}
