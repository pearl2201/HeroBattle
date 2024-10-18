using NodaTime;
using NpgsqlTypes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.BattlePasses
{
    public class BattlePassSeason : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(DefinitionId))]
        public BattlePassDefinition Definition { get; set; }

        public int DefinitionId { get; set; }

        public NpgsqlRange<Instant> Duration { get; set; }

        public bool Expired { get; set; }

        public List<BattlePassSeasonParticipant> Participants { get; set; } = new();
    }
}
