using MasterServer.Domain.Entities.Economy;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassMilestone : BaseAuditableEntity
    {
        [ForeignKey(nameof(SeasonId))]
        public BattlePassDefinition Season { get; set; }

        public int SeasonId { get; set; }
        public int MilestoneId { get; set; }

        [ForeignKey(nameof(ItemId))]
        public BaseEconomyDefinition Item { get; set; }
        public string ItemId { get; set; }
        public int Amount { get; set; }

        public int RequiredTrophy { get; set; }

        [ForeignKey(nameof(PurchaseDefinitionId))]
        public VirtualPurchaseDefinition PurchaseDefinition { get; set; }

        public string PurchaseDefinitionId { get; set; }

        public List<BattlePassSeasonParticipantMilestone> MilestoneParticipants { get; set; }
    }
}
