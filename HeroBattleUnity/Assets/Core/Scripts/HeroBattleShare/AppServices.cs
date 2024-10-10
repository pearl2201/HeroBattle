using FixMath.NET;
using HeroBattle.FixedMath;
using HeroBattle;
using HeroBattleShare.Factory;
using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroBattleShare
{
    public class AppServices
    {
        private static AppServices _instance;
        public IGameFactorySystem GameFactorySystem { get; set; }

        public IGameInputSystem GameInputSystem { get; set; }
        public static AppServices Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AppServices();
                }
                return _instance;
            }
        }
        public AppServices()
        {

        }

        public EntityTypesMap<GameEntities> RegisterTypeMap()
        {
            EntityManager.RegisterFieldType<Vector2f>();
            EntityManager.RegisterFieldType<Vector3f>();
            var typesMap = new EntityTypesMap<GameEntities>()
                 .Register(GameEntities.Player, e => new BaseCharacter(e))
                 .Register(GameEntities.PlayerController, e => new BaseCharacterController(e))
                 .Register(GameEntities.Minion, e => new BaseMinion(e))
                 .Register(GameEntities.BotController, e => new ServerBotController(e))
                 .Register(GameEntities.Random, e => new RandomManager(e))
                ;
            return typesMap;

        }
    }
}
