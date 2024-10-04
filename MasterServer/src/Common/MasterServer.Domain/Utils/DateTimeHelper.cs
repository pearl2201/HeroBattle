using NodaTime;

namespace MasterServer.Domain.Utils
{
    public static class DateTimeHelper
    {
        public static DateTime Now => DateTime.UtcNow;

        public static Instant InstanceNow => Instant.FromDateTimeUtc(Now);

        public static Instant InstanceDate => Instant.FromDateTimeUtc(Now.Date);

        public static Instant ToUtcInstance(DateTime time) => Instant.FromDateTimeUtc(DateTime.SpecifyKind(time, DateTimeKind.Utc));

        public static Instant ToInstantUtc(this DateTime time) => Instant.FromDateTimeUtc(DateTime.SpecifyKind(time, DateTimeKind.Utc));

        public static DateTime? ToDateTimeUtc(this Instant? time) => time == null ? null : time.Value.ToDateTimeUtc();
    }
}
