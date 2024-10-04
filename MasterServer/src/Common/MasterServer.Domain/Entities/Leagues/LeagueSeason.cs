using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Leagues
{
    public class LeagueSeason : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public Instant StartedAt { get; set; }

        public Instant EndedAt { get; set; }

        public Instant? PublishedAt { get; set; }

        public List<LeagueSeasonParticipant> Participants { get; set; }

        public List<LeagueSeasonRank> Ranks { get; set; }
    }
}
