using MasterServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueRank
    {
        [Key]
        public GameLeagueRank RankId { get; set; }

        public GameLeagueTier TierId { get; set; }

        [ForeignKey("TierId")]
        public LeagueTier Tier { get; set; }
    }
}
