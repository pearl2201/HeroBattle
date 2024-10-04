using MasterServer.Domain.Entities.Economy;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassMilestoneReward : IRewardable
    {
        public MilestoneRewardType RewardType { get; set; }
        public int MilestoneType { get; set; }

        public BaseEconomyDefinition Item { get; set; }
        public string ItemId { get; set; }
        public int Amount { get; set; }
    }

    public enum MilestoneRewardType
    {
        Free,
        Premium
    }
}
