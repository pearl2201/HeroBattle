// Copyright (c) 2002-2003, Sony Computer Entertainment America
// Copyright (c) 2002-2003, Craig Reynolds <craig_reynolds@playstation.sony.com>
// Copyright (C) 2007 Bjoern Graf <bjoern.graf@gmx.net>
// Copyright (C) 2007 Michael Coles <michael@digini.com>
// All rights reserved.
//
// This software is licensed as described in the file license.txt, which
// you should have received as part of this distribution. The terms
// are also available at http://www.codeplex.com/SharpSteer/Project/License.aspx.

using System.Collections.Generic;
using System.Numerics;
using HeroBattle.FixedMath;
using SharpSteer2.Helpers;
using SharpSteer2.Obstacles;
using SharpSteer2.Pathway;

namespace SharpSteer2
{
	public abstract class SteerLibrary : BaseVehicle
	{
	    protected IAnnotationService annotation { get; private set; }

	    // Constructor: initializes state
	    protected SteerLibrary(IAnnotationService annotationService = null)
		{
            annotation = annotationService ?? new NullAnnotationService();

			// set inital state
			Reset();
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
	}
}
