using HeroBattle.FixedMath;
using HeroBattleShare;
using HeroBattleShare.Characters;
using HeroBattleShare.Factory;
using LiteEntitySystem;
using SharpSteer2;
using System.Collections.Generic;
using System.Linq;

namespace HeroBattle
{
    [EntityFlags(EntityFlags.Updateable)]
    public class BaseMinion : EntityLogic // SimpleVehicle 
    {
        public BaseCharacterSkillDefinition skillDefinition;
        public BaseCharacterStatDefinition statDefinition;
        public SyncVar<int> _minionId;
        public SyncVar<float> _hp;
        public SyncVar<float> _mana;
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector3f> _position;
        public float Hp
        {
            get { return _hp.Value; }
            set { _hp.Value = value; }
        }

        public float Mana
        {
            get { return _mana.Value; }
            set { _mana.Value = value; }
        }

        public Vector3f Position
        {
            get { return _position.Value; }
            set { _position.Value = value; }
        }


        public Vector3f speed;
        private IBaseMinionView m_View;
        public List<BaseMinion> Enemy { get; set; }
        private readonly List<IVehicle> _neighbours = new List<IVehicle>();

        public const float WORLD_RADIUS = 30;

        private float _lastFired = -100;
        private const float REFIRE_TIME = 2f;

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
                _position.Value = new Vector3f(randomManager.GetRandom(-3, 3), (randomManager.GetRandom(-3, 3)), 0);
                speed = new Vector3f((randomManager.GetRandom(-3, 3)) / (100), (randomManager.GetRandom(-3, 3)) / (100), 0);
            }
        }

        public void Init(int id, int level)
        {
            this._minionId.Value = id;
            var characterDefinition = AppServices.Instance.CharacterRepositiory.GetCharacterDefinition(id);
            skillDefinition = characterDefinition.SkillDefinitions.FirstOrDefault(x => x.level == level);   
            statDefinition = characterDefinition.StatDefinitions.FirstOrDefault(y => y.level == level);
        }

        protected override void Update()
        {
            base.Update();

            if (IsLocal || EntityManager.IsServer)
            {
                if (_mana == skillDefinition.mana)
                {

                }
            }


        }

        public BaseMinion GetClosestEnemy(BaseMinion minion, float attackRange)
        {
            BaseMinion enemy = null;
            float minRange = attackRange;
            float tempRanage = 0;
            var minions = minion.EntityManager.GetEntities<BaseMinion>();
            foreach (var m in minions)
            {
                tempRanage = Vector3f.Distance(minion.Position, m.Position);
                if (tempRanage < minRange)
                {
                    minRange = tempRanage;
                    enemy = m;
                }
            }

            return enemy;
        }


        public void Attack()
        {

        }

        public void Cast()
        {

        }

        public void Move()
        {
            var temp = _position.Value + (speed * EntityManager.DeltaTimeF);
            _position.Value = temp;
        }

        public void OnPhysicalDamage()
        {

        }

        public void OnMagicalDamage()
        {

        }


        public void OnHealth()
        {

        }

        public void OnRaiseSp()
        {

        }

        public void OnFreeze()
        {

        }

        public void OnPoison()
        {

        }
        public void Death()
        {

        }


    }
}