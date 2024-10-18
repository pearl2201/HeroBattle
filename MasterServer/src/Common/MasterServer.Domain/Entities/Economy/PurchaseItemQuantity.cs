using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class PurchaseItemQuantity : BaseAuditableEntity
    {
        [Key]
        public long Id { get; set; }
        [ForeignKey("ItemId")]
        public BaseEconomyDefinition Item { get; set; }

        public string ItemId { get; set; }

        public int Amount { get; set; }
    }
}
