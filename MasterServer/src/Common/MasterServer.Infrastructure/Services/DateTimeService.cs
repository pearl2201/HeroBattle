using MasterServer.Application.Common.Interfaces;
using NodaTime;

namespace MasterServer.Infrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime Now => DateTime.UtcNow;

    public Instant InstanceNow => Instant.FromDateTimeUtc(Now);

    public Instant InstanceDate => Instant.FromDateTimeUtc(Now.Date);

    public Instant ToUtcInstance(DateTime time) => Instant.FromDateTimeUtc(DateTime.SpecifyKind(time, DateTimeKind.Utc));
}
