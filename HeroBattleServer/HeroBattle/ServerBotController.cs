using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattle
{
    public class ServerBotController : AiControllerLogic<BaseCharacter>
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
