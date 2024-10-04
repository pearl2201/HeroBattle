using MasterServer.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerBalance : BaseAuditableEntity
    {
        public CurrencyDefinition Definition { get; set; }


        public Player Player { get; set; }
        public int Amount { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
