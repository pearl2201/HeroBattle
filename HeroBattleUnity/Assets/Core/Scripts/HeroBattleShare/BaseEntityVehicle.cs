using HeroBattle.FixedMath;
using LiteEntitySystem;
using LiteEntitySystem.FixedMath;
using SharpSteer2;
using SharpSteer2.Helpers;
using SharpSteer2.Obstacles;
using SharpSteer2.Pathway;
using System.Collections.Generic;

namespace HeroBattleShare
{
    public abstract class BaseEntityVehicle : EntityLogic, IVehicle
    {
        protected IAnnotationService annotation { get; private set; }
        public BaseEntityVehicle(EntityParams entityParams) : base(entityParams)
        {
            annotation = new NullAnnotationService();

            // set inital state
            Reset();
        }

        public abstract float Mass { get; set; }
        public abstract float Radius { get; set; }
        public abstract Vector3f Velocity { get; }
        public abstract Vector3f Acceleration { get; }
        public abstract float Speed { get; set; }

        public abstract Vector3f PredictFuturePosition(float predictionTime);

        public abstract float MaxForce { get; }
        public abstract float MaxSpeed { get; }

        public Vector3f SideField;

        /// <summary>
        /// side-pointing unit basis vector
        /// </summary>
        public Vector3f Side
        {
            get { return SideField; }
            set { SideField = value; }
        }

        public Vector3f UpField;

        /// <summary>
        /// upward-pointing unit basis vector
        /// </summary>
        public Vector3f Up
        {
            get { return UpField; }
            set { UpField = value; }
        }

        public Vector3f ForwardField;

        /// <summary>
        /// forward-pointing unit basis vector
        /// </summary>
        public Vector3f Forward
        {
            get { return ForwardField; }
            set { ForwardField = value; }
        }

        public Vector3f PositionField;

        /// <summary>
        /// origin of local space
        /// </summary>
        public Vector3f Position
        {
            get { return PositionField; }
            set { PositionField = value; }
        }


        // reset state
        public virtual void Reset()
        {
            // initial state of wander behavior
            _wanderSide = 0;
            _wanderUp = 0;
        }

        #region steering behaviours
        private float _wanderSide;
        private float _wanderUp;
        public Vector3f SteerForWander(float dt)
        {
            return this.SteerForWander(dt, ref _wanderSide, ref _wanderUp);
        }

        public Vector3f SteerForFlee(Vector3f target)
        {
            return this.SteerForFlee(target, MaxSpeed);
        }

        public Vector3f SteerForSeek(Vector3f target)
        {
            return this.SteerForSeek(target, MaxSpeed);
        }

        public Vector3f SteerForArrival(Vector3f target, float slowingDistance)
        {
            return this.SteerForArrival(target, MaxSpeed, slowingDistance, annotation);
        }

        public Vector3f SteerToFollowFlowField(IFlowField field, float predictionTime)
        {
            return this.SteerToFollowFlowField(field, MaxSpeed, predictionTime, annotation);
        }

        public Vector3f SteerToFollowPath(bool direction, float predictionTime, IPathway path)
        {
            return this.SteerToFollowPath(direction, predictionTime, path, MaxSpeed, annotation);
        }

        public Vector3f SteerToStayOnPath(float predictionTime, IPathway path)
        {
            return this.SteerToStayOnPath(predictionTime, path, MaxSpeed, annotation);
        }

        public Vector3f SteerToAvoidObstacle(float minTimeToCollision, IObstacle obstacle)
        {
            return this.SteerToAvoidObstacle(minTimeToCollision, obstacle, annotation);
        }

        public Vector3f SteerToAvoidObstacles(float minTimeToCollision, IEnumerable<IObstacle> obstacles)
        {
            return this.SteerToAvoidObstacles(minTimeToCollision, obstacles, annotation);
        }

        public Vector3f SteerToAvoidNeighbors(float minTimeToCollision, IEnumerable<IVehicle> others)
        {
            return this.SteerToAvoidNeighbors(minTimeToCollision, others, annotation);
        }

        public Vector3f SteerToAvoidCloseNeighbors<TVehicle>(float minSeparationDistance, IEnumerable<TVehicle> others) where TVehicle : IVehicle
        {
            return this.SteerToAvoidCloseNeighbors<TVehicle>(minSeparationDistance, others, annotation);
        }

        public Vector3f SteerForSeparation(float maxDistance, float cosMaxAngle, IEnumerable<IVehicle> flock)
        {
            return this.SteerForSeparation(maxDistance, cosMaxAngle, flock, annotation);
        }

        public Vector3f SteerForAlignment(float maxDistance, float cosMaxAngle, IEnumerable<IVehicle> flock)
        {
            return this.SteerForAlignment(maxDistance, cosMaxAngle, flock, annotation);
        }

        public Vector3f SteerForCohesion(float maxDistance, float cosMaxAngle, IEnumerable<IVehicle> flock)
        {
            return this.SteerForCohesion(maxDistance, cosMaxAngle, flock, annotation);
        }

        public Vector3f SteerForPursuit(IVehicle quarry, float maxPredictionTime = float.MaxValue)
        {
            return this.SteerForPursuit(quarry, maxPredictionTime, MaxSpeed, annotation);
        }

        public Vector3f SteerForEvasion(IVehicle menace, float maxPredictionTime)
        {
            return this.SteerForEvasion(menace, maxPredictionTime, MaxSpeed, annotation);
        }

        public Vector3f SteerForTargetSpeed(float targetSpeed)
        {
            return this.SteerForTargetSpeed(targetSpeed, MaxForce, annotation);
        }
        #endregion

        //public LocalSpace(Vector3f up, Vector3f forward, Vector3f position)
        //{
        //    Up = up;
        //    Forward = forward;
        //    Position = position;
        //    SetUnitSideFromForwardAndUp();
        //}

        //public LocalSpace(Matrix4x4f transformation)
        //{
        //    LocalSpaceBasisHelpers.FromMatrix(transformation, out ForwardField, out SideField, out UpField, out PositionField);
        //}

        // ------------------------------------------------------------------------
        // reset transform: set local space to its identity state, equivalent to a
        // 4x4 homogeneous transform like this:
        //
        //     [ X 0 0 0 ]
        //     [ 0 1 0 0 ]
        //     [ 0 0 1 0 ]
        //     [ 0 0 0 1 ]
        //
        // where X is 1 for a left-handed system and -1 for a right-handed system.
        public void ResetLocalSpace()
        {
            LocalSpaceBasisHelpers.ResetLocalSpace(out ForwardField, out SideField, out UpField, out PositionField);
        }

        // ------------------------------------------------------------------------
        // set "side" basis vector to normalized cross product of forward and up
        public void SetUnitSideFromForwardAndUp()
        {
            LocalSpaceBasisHelpers.SetUnitSideFromForwardAndUp(ref ForwardField, out SideField, ref UpField);
        }

        // ------------------------------------------------------------------------
        // regenerate the orthonormal basis vectors given a new forward
        //(which is expected to have unit length)
        public void RegenerateOrthonormalBasisUF(Vector3f newUnitForward)
        {
            LocalSpaceBasisHelpers.RegenerateOrthonormalBasisUF(newUnitForward, out ForwardField, out SideField, ref UpField);
        }

        // for when the new forward is NOT know to have unit length
        public void RegenerateOrthonormalBasis(Vector3f newForward)
        {
            LocalSpaceBasisHelpers.RegenerateOrthonormalBasis(newForward, out ForwardField, out SideField, ref UpField);
        }

        // for supplying both a new forward and and new up
        public void RegenerateOrthonormalBasis(Vector3f newForward, Vector3f newUp)
        {
            LocalSpaceBasisHelpers.RegenerateOrthonormalBasis(newForward, newUp, out ForwardField, out SideField, out UpField);
        }
    }
}
