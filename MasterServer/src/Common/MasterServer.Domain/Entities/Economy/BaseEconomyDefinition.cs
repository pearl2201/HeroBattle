using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Economy
{
    public class BaseEconomyDefinition : BaseAuditableEntity
    {
        [Key]
        public string Id { get; set; }

        public string Name { get; set; }

        public int Initial { get; set; }

        public int Max { get; set; }

        public Dictionary<string, object> CustomData { get; set; }
        public virtual EconomyType EconomyType { get; }
    }

    public enum EconomyType
    {
        Currency,
        InventoryItem,
        RealMoneyPurchase,
        VirtualPurchase
    }
}
