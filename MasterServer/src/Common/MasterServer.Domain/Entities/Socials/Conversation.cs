using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Socials
{
    public class Conversation : BaseAuditableEntity
    {
        [Key]
        public Guid Id { get; set; }

        public List<ConversationParticipant> ConversationParticipants { get; set; }
    }
}
