using MasterServer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Domain.Entities.Economy
{
    public class StoreIdentifer : BaseAuditableEntity
    {
        public string ProductId { get; set; }

        public PublisherPlatform Platform { get; set; }
    }
}
