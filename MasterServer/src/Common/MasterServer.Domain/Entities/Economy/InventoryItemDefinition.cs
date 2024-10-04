using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MasterServer.Domain.Entities.Economy
{
    public class InventoryItemDefinition : BaseEconomyDefinition
    {
        public override EconomyType EconomyType => EconomyType.InventoryItem;
    }
}
