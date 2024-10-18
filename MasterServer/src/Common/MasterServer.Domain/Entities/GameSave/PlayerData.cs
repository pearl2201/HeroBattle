using MasterServer.Domain.Entities.Leaderboard;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.GameSave
{
    public class PlayerData : BaseAuditableEntity
    {
        public long PlayerId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public string Key { get; set; }

        public Dictionary<string, object> Value { get; set; }
    }
}
