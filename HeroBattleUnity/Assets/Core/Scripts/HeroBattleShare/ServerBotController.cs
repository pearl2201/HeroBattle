using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattle
{
    public class ServerBotController : AiControllerLogic<BasePlayer>
    {
        public ServerBotController(EntityParams entityParams) : base(entityParams)
        {
        }

        public override void BeforeControlledUpdate()
        {
            ControlledEntity.SetInput();
        }
    }
}
