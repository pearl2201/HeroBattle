using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Socials
{
    public class ConversationParticipant : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public long PlayerId { get; set; }

        [ForeignKey("PlayerId")]
        public Player Player { get; set; }

        public Guid ConversationId { get; set; }
        [ForeignKey("ConversationId")]
        public Conversation Conversation { get; set; }

        public List<ConversationMessage> SentMessages { get; set; }
    }
}
