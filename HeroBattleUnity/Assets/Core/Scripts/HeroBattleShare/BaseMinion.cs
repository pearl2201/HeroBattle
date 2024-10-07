using FixMath.NET;
using HeroBattle.FixedMath;
using HeroBattleShare;
using HeroBattleShare.Factory;
using LiteEntitySystem;
using SharpSteer2.Database;
using SharpSteer2;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using SharpSteer2.Helpers;
using System.Drawing;
using System;

namespace HeroBattle
{
    [EntityFlags(EntityFlags.Updateable)]
    public class BaseMinion : EntityLogic // SimpleVehicle 
    {

        private SimpleVehicle vehicle;
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector2f> position;
        public Vector2f speed;
        private IBaseMinionView m_View;
        private readonly ITokenForProximityDatabase<IVehicle> _proximityToken;
        public List<BaseMinion> Enemy { get; set; }
        private readonly List<IVehicle> _neighbours = new List<IVehicle>();

        public override float MaxForce
        {
            get { return 7; }
        }
        public override float MaxSpeed
        {
            get { return 15; }
        }

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

            _neighbours.Clear();
            _proximityToken.FindNeighbors(Position, 50, _neighbours);
            var target = ClosestEnemy(_neighbours);
            {
                // attack
            }


            Vector3 otherPlaneForce = vehicle.SteerToAvoidCloseNeighbors(3, _neighbours);
            if (target != null)
                otherPlaneForce += vehicle.SteerForPursuit(target);

            var boundary = HandleBoundary();

            //var evasion = _neighbours
            //    .Where(v => v is Missile)
            //    .Cast<Missile>()
            //    .Where(m => m.Target == this)
            //    .Select(m => SteerForEvasion(m, 1))
            //    .Aggregate(Vector3.Zero, (a, b) => a + b);

            vehicle.ApplySteeringForce(otherPlaneForce + boundary + vehicle.SteerForWander(elapsedTime) * 0.1f, elapsedTime);

            _proximityToken.UpdateForNewPosition(Position);
        }

        private Vector3 HandleBoundary()
        {
            // while inside the sphere do noting
            if (Position.Length() < WORLD_RADIUS)
                return Vector3.Zero;

            // steer back when outside
            Vector3 seek = vehicle.SteerForSeek(Vector3.Zero);
            Vector3 lateral = Vector3Helpers.PerpendicularComponent(seek, Forward);
            return lateral;

        }

        protected override void VisualUpdate()
        {
#if UNITY

#endif
        }

    }
}