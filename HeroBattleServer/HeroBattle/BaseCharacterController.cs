using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Text;
using static HeroBattle.GamePackets;

namespace HeroBattle
{
    public class BaseCharacterController : HumanControllerLogic<PlayerInputPacket, BaseCharacter>
    {
        private PlayerInputPacket _nextCommand;
        public BaseCharacterController(EntityParams entityParams) : base(entityParams)
        {
        }

        protected override void Update()
        {
            base.Update();
            if (ControlledEntity == null)
                return;
        }

        protected override void ReadInput(in PlayerInputPacket input)
        {
            
        }

        protected override void GenerateInput(out PlayerInputPacket input)
        {
            input = _nextCommand;
            _nextCommand.Keys = 0;
        }
    }
}
