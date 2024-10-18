using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class RealMoneyPurchaseDefinition : BaseEconomyDefinition
    {
        public override EconomyType EconomyType => EconomyType.RealMoneyPurchase;

        public List<RealMoneyPurchaseReward> Rewards { get; set; }

        public List<StoreIdentifer> StoreIdentifers { get; set; }
    }
}
