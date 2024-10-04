using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterServer.Application.Common.Interfaces
{
    public interface IRandomService
    {
        public int Random(string tag, int start, int end);

        public int RandomMaxInclusive(string tag, int start, int end);

        public float Random(string tag, float start, float end);

        public float RandomMaxInclusive(string tag, float start, float end);

        public T Random<T>(string tag, Dictionary<T, float> map);

        float Random01Ins(string tag);

        string RandomStringOfLength(string tag, int length, bool uppercase = true, bool lowercase = true, bool digits = true);
    }

    public class RandomService : IRandomService
    {
        private readonly Random _random;
        private readonly ILogger<RandomService> _logger;
        private const string uppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        private const string lowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
        private const string digitLetters = "0123456789";
        public RandomService(ILogger<RandomService> logger)
        {
            _random = new Random();
            _logger = logger;
        }
        public int Random(string tag, int start, int end)
        {
            return _random.Next(start, end);
        }

        public float Random(string tag, float start, float end)
        {
            return _random.NextSingle() * (end - start) + start;
        }

        public int RandomMaxInclusive(string tag, int start, int end)
        {
            return _random.Next(start, end + 1);
        }

        public float Random01Ins(string tag)
        {
            return randomFloat(0f, 1f, 0.01f);
        }

        public float RandomMaxInclusive(string tag, float start, float end)
        {
            return randomFloat(start, end, 0.01f);
        }

        public static float randomFloat(float minInclusive, float maxInclusive, float precision)
        {
            int max = (int)(maxInclusive / precision);
            int min = (int)(minInclusive / precision);
            Random rand = new Random();
            int randomInt = rand.Next((max - min) + 1) + min;
            float randomNum = randomInt * precision;
            return randomNum;
        }


        public T Random<T>(string tag, Dictionary<T, float> map)
        {
            var labels = map.Select(x => x.Key).ToList();
            var weights = map.Select(x => x.Value).ToList();
            var random = Random("hero rarity", 0f, map.Select(x => x.Value).Sum());
            int selected = -1;
            int idx = 0;
            float upper = 0;
            while (selected == -1 && idx < map.Count)
            {
                upper += weights[idx];
                if (random <= upper)
                {
                    selected = idx;
                    break;
                }
                idx++;
            }
            if (selected == -1)
            {
                return labels[0];
            }

            return labels[selected];
        }

        public string RandomStringOfLength(string tag, int length, bool uppercase = true, bool lowercase = true, bool digit = true)
        {

            string allowedChars = "";
            if (uppercase)
            {
                allowedChars = allowedChars + uppercaseLetters;
            }
            if (lowercase)
            {
                allowedChars = allowedChars + lowercaseLetters;
            }
            if (digit)
            {
                allowedChars = allowedChars + digit;
            }
            var stringBuilder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                stringBuilder.Append(allowedChars[_random.Next(0, allowedChars.Length)]);
            }
            return stringBuilder.ToString();
        }
    }
}
