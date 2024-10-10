using HeroBattle.FixedMath;
using HeroBattleShare;
using HeroBattleShare.Factory;
using LiteEntitySystem;
using SharpSteer2;
using SharpSteer2.Database;
using SharpSteer2.Helpers;
using System;
using System.Collections.Generic;

namespace HeroBattle
{
    [EntityFlags(EntityFlags.Updateable)]
    public class BaseMinion : EntityLogic // SimpleVehicle 
    {

        private SimpleVehicle vehicle;
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector3f> _position;
        public Vector3f speed;
        private IBaseMinionView m_View;
        private readonly ITokenForProximityDatabase<IVehicle> _proximityToken;
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

        protected override void Update()
        {
            base.Update();

            if (IsLocal || EntityManager.IsServer)
            {
                var temp = _position.Value + (speed * EntityManager.DeltaTimeF);
                _position.Value = temp;
            }

            _neighbours.Clear();
            _proximityToken.FindNeighbors(_position.Value, 50, _neighbours);
            var target = ClosestEnemy(_neighbours);
            {
                // attack
            }


            Vector3f otherPlaneForce = vehicle.SteerToAvoidCloseNeighbors(3, _neighbours);
            if (target != null)
                otherPlaneForce += vehicle.SteerForPursuit(target);

            var boundary = HandleBoundary();

            //var evasion = _neighbours
            //    .Where(v => v is Missile)
            //    .Cast<Missile>()
            //    .Where(m => m.Target == this)
            //    .Select(m => SteerForEvasion(m, 1))
            //    .Aggregate(Vector3.Zero, (a, b) => a + b);

            vehicle.ApplySteeringForce(otherPlaneForce + boundary + vehicle.SteerForWander(EntityManager.DeltaTimeF) * 0.1f, EntityManager.DeltaTimeF);
            _position.Value = vehicle.Position;
            _proximityToken.UpdateForNewPosition(_position);
        }

        private IVehicle ClosestEnemy(List<IVehicle> neighbours)
        {
            throw new NotImplementedException();
        }

        private Vector3f HandleBoundary()
        {
            // while inside the sphere do noting
            if (_position.Value.Length() < WORLD_RADIUS)
                return Vector3f.Zero;

            // steer back when outside
            Vector3f seek = vehicle.SteerForSeek(Vector3f.Zero);
            Vector3f lateral = Vector3fHelpers.PerpendicularComponent(seek, vehicle.Forward);
            return lateral;

        }

        protected override void VisualUpdate()
        {
#if UNITY

#endif
        }

    }
}