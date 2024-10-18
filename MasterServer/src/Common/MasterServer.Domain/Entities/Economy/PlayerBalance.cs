using MasterServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerBalance : BaseAuditableEntity
    {

        [ForeignKey(nameof(DefinitionId))]
        public CurrencyDefinition Definition { get; set; }

        public string DefinitionId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public int PlayerId     { get; set; }
        public int Amount { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
