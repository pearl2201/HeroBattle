using MasterServer.Domain.Enums;
using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.GameNotification
{
    public class GameNotification
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public Instant StartedAt { get; set; }

        public Instant? ExpiredAt { get; set; }

        public GameMailTargetKind TargetKind { get; set; }

        public Instant? PublishedAt { get; set; }
    }
}
