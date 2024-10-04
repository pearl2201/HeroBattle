using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class VirtualPurchaseDefinition : BaseEconomyDefinition
    {
        public override EconomyType EconomyType => EconomyType.VirtualPurchase;

        public List<PurchaseItemQuantity> Costs { get; set; }

        public List<PurchaseItemQuantity> Rewards { get; set; }
    }
}
