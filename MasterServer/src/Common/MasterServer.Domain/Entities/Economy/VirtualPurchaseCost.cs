using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Economy
{
    public class VirtualPurchaseCost: BaseAuditableEntity
    {
        public string VirtualPurchaseDefinitionId { get; set; }
        [ForeignKey(nameof(VirtualPurchaseDefinitionId))]
        public VirtualPurchaseDefinition VirtualPurchaseDefinition { get; set; }

        public int PurchaseItemQuantityId { get; set; }
        [ForeignKey(nameof(PurchaseItemQuantityId))]
        public PurchaseItemQuantity PurchaseItemQuantity { get; set; }
    }
}
