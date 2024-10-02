using HeroBattle.FixedMath;
using LiteEntitySystem;

namespace HeroBattle
{
    [EntityFlags(EntityFlags.Updateable)]
    public class BaseMinion : EntityLogic
    {
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector2f> position;
        public Vector2f speed;
        public BaseMinion(EntityParams entityParams) : base(entityParams)
        {

        }

        protected override void Update()
        {
            base.Update();

            if (IsLocal || EntityManager.IsServer)
            {
                var temp = position.Value + (speed * EntityManager.DeltaTimeF);
                position.Value = temp;
            }
        }

        protected override void VisualUpdate()
        {
#if UNITY

#endif
        }
    }
}
