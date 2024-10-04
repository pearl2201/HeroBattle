using System.ComponentModel.DataAnnotations;

namespace MasterServer.Domain.Entities.Mails
{
    public class GameMailTemplate : BaseAuditableEntity
    {
        [Key]
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string TemplateName { get; set; }

        public string TemplateDescription { get; set; }

        public string TemplateImagePath { get; set; }
    }
}
