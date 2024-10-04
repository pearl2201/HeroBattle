using MasterServer.Domain.Entities.Economy;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Mails
{
    public class MailAttachment : BaseAuditableEntity, IRewardable
    {
        [Key]
        public int Id { get; set; }

        public int Amount { get; set; }

        public int MailId { get; set; }
        [ForeignKey("MailId")]
        public GameMail Mail { get; set; }

        [ForeignKey("ItemId")]
        public BaseEconomyDefinition Item { get; set; }
        public string ItemId { get; set; }
    }
}
