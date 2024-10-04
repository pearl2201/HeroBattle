using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Economy
{
    public class PlayerShopVirtualPurchaseReceipt : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }
        public long PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public string PurchaseDefinitionId { get; set; }
        [ForeignKey("PurchaseDefinitionId")]
        public VirtualPurchaseDefinition PurchaseDefinition { get; set; }
        [Column(TypeName = "jsonb")]
        public List<RewardItemValueObject> Rewards { get; set; }
    }
}
