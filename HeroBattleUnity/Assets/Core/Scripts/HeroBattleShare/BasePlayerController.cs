using FixMath.NET;
using HeroBattleShare;
using LiteEntitySystem;
using System;
using System.Collections.Generic;
using System.Text;
using static HeroBattle.GamePackets;

namespace HeroBattle
{
    public class BasePlayerController : HumanControllerLogic<PlayerInputPacket, BasePlayer>
    {
        private PlayerInputPacket _nextCommand;

        public BasePlayerController(EntityParams entityParams) : base(entityParams)
        {
        }

        protected override void Update()
        {
            base.Update();
            if (ControlledEntity == null)
                return;
        }

        protected override void VisualUpdate()
        {
            if (ControlledEntity == null)
                return;
            var temp = AppServices.Instance.GameInputSystem.GetPlayerInput();
            Vector2 dir = mousePos - ControlledEntity.Position;
            float rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            if (velocity.x < -0.5f)
                _nextCommand.Keys |= MovementKeys.Left;
            if (velocity.x > 0.5f)
                _nextCommand.Keys |= MovementKeys.Right;
            if (velocity.y < -0.5f)
                _nextCommand.Keys |= MovementKeys.Up;
            if (velocity.y > 0.5f)
                _nextCommand.Keys |= MovementKeys.Down;
        }
        protected override void ReadInput(in PlayerInputPacket input)
        {

            if (temp.x > 0)
            {
                input.Keys = input.Keys | MovementKeys.Right;
            }

            ControlledEntity?.SetInput(
    input.Keys.HasFlagFast(MovementKeys.Fire),
    input.Keys.HasFlagFast(MovementKeys.Projectile),
    input.Rotation,
    velocity);
        }

        protected override void GenerateInput(out PlayerInputPacket input)
        {
            input = _nextCommand;
            _nextCommand.Keys = 0;
        }
    }
}
