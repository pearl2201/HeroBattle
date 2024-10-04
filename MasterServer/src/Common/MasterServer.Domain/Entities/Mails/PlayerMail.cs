using NodaTime;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Mails
{
    public class PlayerMail : BaseAuditableEntity
    {
        public int MailId { get; set; }

        public long PlayerId { get; set; }

        [ForeignKey("MailId")]
        public GameMail Mail { get; set; }
        [ForeignKey("PlayerId")]
        public Player Player { get; set; }
        [ConcurrencyCheck]
        public Instant? ReceivedAt { get; set; }

        public Instant? ReadedAt { get; set; }
    }
}
