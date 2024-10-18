using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class RealMoneyPurchaseReward : BaseAuditableEntity
    {
        public string VirtualPurchaseDefinitionId { get; set; }
        [ForeignKey(nameof(VirtualPurchaseDefinitionId))]
        public VirtualPurchaseDefinition VirtualPurchaseDefinition { get; set; }

        public int PurchaseItemQuantityId { get; set; }
        [ForeignKey(nameof(PurchaseItemQuantityId))]
        public PurchaseItemQuantity PurchaseItemQuantity { get; set; }
    }
}
