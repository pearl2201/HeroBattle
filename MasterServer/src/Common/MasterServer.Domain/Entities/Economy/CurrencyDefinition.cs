using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MasterServer.Domain.Entities.Economy
{
    public class CurrencyDefinition : BaseEconomyDefinition
    {
        public override EconomyType EconomyType => EconomyType.Currency;
    }
}
