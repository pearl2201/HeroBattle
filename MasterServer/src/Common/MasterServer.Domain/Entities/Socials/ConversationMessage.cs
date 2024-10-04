using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Socials
{
    public class ConversationMessage : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string MessageText { get; set; }

        [ForeignKey("SenderId")]
        public ConversationParticipant Sender { get; set; }

        public Guid SenderId { get; set; }
    }
}
