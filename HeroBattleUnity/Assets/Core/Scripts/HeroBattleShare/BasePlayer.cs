﻿using HeroBattle.FixedMath;
using LiteEntitySystem;
using LiteEntitySystem.Extensions;
using SharpSteer2;
using static HeroBattle.GamePackets;

namespace HeroBattle
{
    public class BasePlayer : PawnLogic
    {
        [SyncVarFlags(SyncFlags.Interpolated)]
        public SyncVar<Vector2f> _position;
        [SyncVarFlags(SyncFlags.Interpolated | SyncFlags.LagCompensated)]
        private SyncVar<FloatAngle> _rotation;

        [SyncVarFlags(SyncFlags.AlwaysRollback)]
        private SyncVar<byte> _health;
        public SyncVar<Vector2f> speed;

        private SimpleVehicle _vehicle;
        public byte Health => _health;
        public Vector2f Position => _position.Value;
        public float Rotation => _rotation.Value;

        private static RemoteCall<AttackPacket> _attackRemoteCall;
        private static RemoteCall<HitPacket> _hitRemoteCall;
        public BasePlayer(EntityParams entityParams) : base(entityParams)
        {
        }

        protected override void RegisterRPC(ref RPCRegistrator r)
        {
            base.RegisterRPC(ref r);
            r.CreateRPCAction(this, OnAttack, ref _attackRemoteCall, ExecuteFlags.ExecuteOnPrediction | ExecuteFlags.SendToOther);
            r.CreateRPCAction(this, OnHit, ref _hitRemoteCall, ExecuteFlags.ExecuteOnPrediction | ExecuteFlags.SendToOther);
        }



        public void SetInput()
        {

        }

        public void OnAttack(AttackPacket pkt)
        {

        }

        public void OnHit(HitPacket pkt)
        {

        }


        public void SetInput(float rotation, Vector2f velocity)
        {

        }

        protected override void Update()
        {

        }
    }
}
