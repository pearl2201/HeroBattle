using LiteEntitySystem;
using SharpSteer2;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroBattleShare
{
    public abstract  class BaseEntityVehicle : EntityLogic, IVehicle
    {
        public BaseEntityVehicle(EntityParams entityParams) : base(entityParams)
        {
        }

        public abstract float Mass { get; set; }
        public abstract float Radius { get; set; }
        public abstract Vector3 Velocity { get; }
        public abstract Vector3 Acceleration { get; }
        public abstract float Speed { get; set; }

        public abstract Vector3 PredictFuturePosition(float predictionTime);

        public abstract float MaxForce { get; }
        public abstract float MaxSpeed { get; }

        public Vector3 Side => new Vector3f()

        public Vector3 Up => throw new NotImplementedException();

        public Vector3 Forward => throw new NotImplementedException();

        public abstract Vector3 Position { get; set; }
    }
}
