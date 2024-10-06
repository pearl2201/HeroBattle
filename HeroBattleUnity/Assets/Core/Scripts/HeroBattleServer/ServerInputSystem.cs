using HeroBattle.FixedMath;
using HeroBattleShare.Factory;

namespace HeroBattleServer.Factory
{
    public class ServerInputSystem : IGameInputSystem
    {
        public Vector2f GetPlayerInput()
        {
           return new Vector2f(new FixMath.NET.Fix64(0), new FixMath.NET.Fix64(0));
        }
    }
}