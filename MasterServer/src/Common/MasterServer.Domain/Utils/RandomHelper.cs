using System.Text;

namespace MasterServer.Domain.Utils
{
    public static class RandomHelper
    {
        public static string RandomNumberStringLength(int length)
        {
            const string AllowedChars = "0123456789";
            Random rng = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                sb.Append(AllowedChars[rng.Next(AllowedChars.Length)]);
            }
            return sb.ToString();
        }
        public static string RandomStringWithLength(int length)
        {
            const string AllowedChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz#@$^*()";
            Random rng = new Random();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < length; i++)
            {

                sb.Append(AllowedChars[rng.Next(AllowedChars.Length)]);
            }
            return sb.ToString();
        }
        private static IEnumerable<string> NextStrings(
                this Random rnd,
                string allowedChars,
                (int Min, int Max) length,
                int count)
        {
            ISet<string> usedRandomStrings = new HashSet<string>();
            (int min, int max) = length;
            char[] chars = new char[max];
            int setLength = allowedChars.Length;

            while (count-- > 0)
            {
                int stringLength = rnd.Next(min, max + 1);

                for (int i = 0; i < stringLength; ++i)
                {
                    chars[i] = allowedChars[rnd.Next(setLength)];
                }

                string randomString = new string(chars, 0, stringLength);

                if (usedRandomStrings.Add(randomString))
                {
                    yield return randomString;
                }
                else
                {
                    count++;
                }
            }
        }
    }
}
