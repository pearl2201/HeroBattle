using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerInventory : BaseAuditableEntity
    {
        [ForeignKey(nameof(DefinitionId))]
        public InventoryItemDefinition Definition { get; set; }

        public string DefinitionId { get; set; }
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public int PlayerId { get; set; }
        public int Amount { get; set; }

        [Timestamp]
        public byte[] Timestamp { get; set; }
    }
}
