using MasterServer.Domain.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Socials
{
    public class PlayerEdge : BaseAuditableEntity
    {
        public long SrcPlayerId { get; set; }

        public long DstPlayerId { get; set; }

        [ForeignKey("SrcPlayerId")]
        public Player SrcPlayer { get; set; }
        [ForeignKey("DstPlayerId")]
        public Player DstPlayer { get; set; }

        public EdgeStatus Status { get; set; }

        public string RequestText { get; set; }

        public Conversation Conversation { get; set; }

        public Guid? ConversationId { get; set; }

    }
}
