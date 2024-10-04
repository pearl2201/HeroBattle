using NodaTime;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.GameSave
{
    public class PlayerShopProfile
    {
        public Instant? PurchaseStarterPackAt { get; set; }
    }
}
