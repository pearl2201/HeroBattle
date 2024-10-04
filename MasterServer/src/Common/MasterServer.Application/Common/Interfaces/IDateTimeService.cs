using NodaTime;

namespace MasterServer.Application.Common.Interfaces;

public interface IDateTimeService
{
    DateTime Now { get; }

    Instant InstanceNow { get; }

    Instant InstanceDate { get; }

    Instant ToUtcInstance(DateTime time);
}
