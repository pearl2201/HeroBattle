using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.GameSave
{
    public class PlayerFile : BaseAuditableEntity
    {
        [ForeignKey(nameof(PlayerId))]
        public Player Player { get; set; }

        public long PlayerId { get; set; }

        public string Key { get; set; }

        public string StoragePath { get; set; }

        public string DistributedUrl { get; set; }
    }
}
