namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassSeasonParticipantMilestone : BaseAuditableEntity
    {
        public BattlePassMilestone BattlePassMilestone { get; set; }
        public int MilestoneId { get; set; }

        public long PlayerId { get; set; }

        public int SeasonId { get; set; }

        public BattlePassSeasonParticipant Participant { get; set; }
    }
}
