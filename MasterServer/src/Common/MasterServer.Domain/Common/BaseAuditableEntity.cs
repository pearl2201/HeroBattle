using NodaTime;

namespace MasterServer.Domain.Common;

public abstract class BaseAuditableEntity : BaseEntity
{
    public Instant CreatedAt { get; set; }

    public Instant? UpdatedAt { get; set; }
}
