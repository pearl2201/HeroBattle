using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerInventory : BaseAuditableEntity
    {
        public InventoryItemDefinition Definition { get; set; }


        public Player Player { get; set; }
        public int Amount { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
