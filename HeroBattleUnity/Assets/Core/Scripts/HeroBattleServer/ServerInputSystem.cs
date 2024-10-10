using HeroBattle.FixedMath;
using HeroBattleShare.Factory;

namespace HeroBattleServer.Factory
{
    public class ServerInputSystem : IGameInputSystem
    {
        public Vector2f GetPlayerInput()
        {
           return new Vector2f(0, 0);
        }
    }
}