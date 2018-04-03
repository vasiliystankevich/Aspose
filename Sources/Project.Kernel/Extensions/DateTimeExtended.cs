using System;

namespace Project.Kernel.Extensions
{
    public static class DateTimeExtended
    {
        public static DateTime PruningMilisecond(this DateTime sender)
        {
            var valueUtc = sender.ToUniversalTime();
            return new DateTime(valueUtc.Year, valueUtc.Month, valueUtc.Day, valueUtc.Hour, valueUtc.Minute,
                valueUtc.Second);
        }

        public static DateTime FromUnixTime(this long timestamp)
        {
            return UnixEpoch.AddSeconds(timestamp);
        }

        public static DateTime UnixEpoch { get; set; } = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
