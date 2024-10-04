using MasterServer.Domain.Entities.BattlePasses;
using MasterServer.Domain.Entities.Economy;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassMilestone
    {

        public int Milestone { get; set; }

        public List<BattlePassMilestoneReward> MilestoneRewards { get; set; }

        public int RequiredTrophy { get; set; }

        public int TotalTrophy { get; set; }

        public bool IsBigReward { get; set; }

        [ForeignKey(nameof(PurchaseDefinitionId))]
        public VirtualPurchaseDefinition PurchaseDefinition { get; set; }

        public string PurchaseDefinitionId { get; set; }

        public List<BattlePassParticipantMilestone> MilestoneParticipants { get; set; }

        public BattlePassSeason Season { get; set; }

        public int SeasonId { get; set; }
    }
}
