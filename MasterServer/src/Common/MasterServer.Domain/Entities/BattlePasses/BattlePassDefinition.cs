using MasterServer.Domain.Entities.Economy;
using MasterServer.Domain.Enums;
using NodaTime;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassDefinition : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool Enabled { get; set; }

        public BattlePassType Type { get; set; }

        [ForeignKey(nameof(PurchaseDefinition))]
        public VirtualPurchaseDefinition PurchaseDefinition { get; set; }

        public string PurchaseDefinitionId { get; set; }
        public NpgsqlRange<Instant> Duration { get; set; }

        public Instant? PublishedAt { get; set; }

        public List<BattlePassMilestone> Milestones { get; set; } = new();

        public BattlePassResetType ResetType { get; set; }

        public Instant? OnetimeReset { get; set; }

        public string ScheduledReset { get; set; }

        public List<BattlePassSeason> Seasons { get; set; }
    }

    public enum BattlePassResetType
    {
        OnetimeBattlePass,
        ManualReset,
        AutoReset
    }
}
