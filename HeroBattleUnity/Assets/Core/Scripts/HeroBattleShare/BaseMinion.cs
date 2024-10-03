using FixMath.NET;
using HeroBattle.FixedMath;
using HeroBattleShare;
using HeroBattleShare.Factory;
using LiteEntitySystem;

namespace HeroBattle
{
    [EntityFlags(EntityFlags.Updateable)]
    public class BaseMinion : EntityLogic
    {
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector2f> position;
        public Vector2f speed;
        private IBaseMinionView m_View;
        protected override void OnConstructed()
        {
            base.OnConstructed();
            m_View = AppServices.Instance.GameFactorySystem.GetBaseMinionView(this);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
        }
        public BaseMinion(EntityParams entityParams) : base(entityParams)
        {
            if (IsServer)
            {
                var randomManager = EntityManager.GetSingleton<RandomManager>();
                position.Value = new Vector2f(new Fix64(randomManager.GetRandom(-3, 3)), new Fix64(randomManager.GetRandom(-3, 3)));
                speed = new Vector2f(new Fix64(randomManager.GetRandom(-3, 3)) / new Fix64(100), new Fix64(randomManager.GetRandom(-3, 3)) / new Fix64(100));
            }
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
