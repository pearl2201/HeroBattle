using LiteEntitySystem;
using LiteEntitySystem.Extensions;
using System;

namespace HeroBattleShare.Factory
{
    public class RandomManager : SingletonEntityLogic
    {
        private readonly SyncList<float> preGenerateSeed;
        private int currentIdx;
        public RandomManager(EntityParams entityParams) : base(entityParams)
        {
            var rng = new Random();
            preGenerateSeed = new SyncList<float>();
            for (int i = 0; i < 1000; i++)
            {
                preGenerateSeed.Add((float)rng.NextDouble());
            }
            currentIdx = 0;
        }

        public int GetRandom(int min, int max)
        {
            var val = preGenerateSeed[currentIdx];
            currentIdx = (currentIdx + 1) % preGenerateSeed.Count;
            return (int)(min + (max - min) * val);
        }

        public float GetRandom(float min, float max)
        {
            var val = preGenerateSeed[currentIdx];
            currentIdx = (currentIdx + 1) % preGenerateSeed.Count;
            return (int)(min + (max - min) * val);
        }
    }
}
