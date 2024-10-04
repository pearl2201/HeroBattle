using System.Numerics;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace MasterServer.Domain.Mics
{
    public static class Helpers
    {
        public static string ToJsonString(this JsonDocument jdoc)
        {
            using (var stream = new MemoryStream())
            {
                Utf8JsonWriter writer = new Utf8JsonWriter(stream, new JsonWriterOptions { Indented = true });
                jdoc.WriteTo(writer);
                writer.Flush();
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        public static int CompareVersion(string aVersion, string bVersion)
        {
            var aV = new Version(aVersion);
            var bV = new Version(bVersion);
            return aV.CompareTo(bV);
        }


        public static T GreatestCommonDivisor<T>(T a, T b) where T : INumber<T>
        {
            while (b != T.Zero)
            {
                var temp = b;
                b = a % b;
                a = temp;
            }

            return a;
        }

        public static T GreatestCommonDivisor<T>(this IEnumerable<T> values) where T : INumber<T>
        {


            return values.Aggregate(GreatestCommonDivisor);
        }

        public static T LeastCommonMultiple<T>(T a, T b) where T : INumber<T>
            => a / GreatestCommonDivisor(a, b) * b;

        public static T LeastCommonMultiple<T>(this IEnumerable<T> values) where T : INumber<T>
            => values.Aggregate(LeastCommonMultiple);

        public static float RoundDown(float number, int decimalPlaces)
        {
            return (float)(Math.Floor(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces));
        }

        public static float RoundUp(float number, int decimalPlaces)
        {
            return (float)(Math.Ceiling(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces));
        }

        public static float Round(float number, int decimalPlaces)
        {
            return (float)(Math.Round(number * Math.Pow(10, decimalPlaces)) / Math.Pow(10, decimalPlaces));
        }

        public static TimeSpan ParseJiraTime(string strTime)
        {
            var regex = new Regex(@"(\d*w)?(\d*d)?(\d*h)?(\d*m?)");
            var match = regex.Match(strTime);
            var timespan = new TimeSpan();
            if (match.Success)
            {
                for (var i = 1; i < match.Groups.Count; i++)
                {
                    var text = match.Groups[i].Value;
                    if (text.Length == 0)
                    {
                        continue;
                    }

                    var strip = text.Substring(0, text.Length - 1);
                    if (text.EndsWith("w"))
                    {
                        timespan = timespan.Add(TimeSpan.FromDays(7 * int.Parse(strip)));
                    }
                    else if (text.EndsWith("d"))
                    {
                        timespan = timespan.Add(TimeSpan.FromDays(int.Parse(strip)));
                    }
                    else if (text.EndsWith("h"))
                    {
                        timespan = timespan.Add(TimeSpan.FromHours(int.Parse(strip)));
                    }
                    else if (text.EndsWith("m"))
                    {
                        timespan = timespan.Add(TimeSpan.FromMinutes(int.Parse(strip)));
                    }
                }

            }
            else
            {
                throw new Exception("Could not validate time format: " + strTime);
            }

            return timespan;
        }

        public static string BuildJiraTimeFromMinutes(int minutes)
        {
            var ts = TimeSpan.FromMinutes(minutes);
            var strBuilder = new StringBuilder();
            if (ts.TotalDays > 7)
            {
                var week = (int)ts.TotalDays / 7;
                ts = ts.Subtract(TimeSpan.FromDays(week * 7));
                strBuilder.Append($"{week / 7}w");
            }
            if (ts.Days > 0)
            {
                strBuilder.Append($"{ts.Days}d");
            }
            if (ts.Hours > 0)
            {
                strBuilder.Append($"{ts.Hours}h");
            }
            if (ts.Minutes > 0)
            {
                strBuilder.Append($"{ts.Minutes}m");
            }
            return strBuilder.ToString();
        }
    }
}
