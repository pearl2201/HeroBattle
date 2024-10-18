using MasterServer.Domain.DomainEvent;
using NodaTime;
using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities
{
    public class GameSession : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }
        public long PlayerId { get; set; }

        public int ConnectionId { get; set; }

        public bool Announced { get; set; }

        public Instant? ConnectedAt { get; set; }

        public Instant? DisconnectedAt { get; set; }

        public GameSession()
        {
            AddDomainEvent(new PlayerLoginEvent());
        }
    }
}
