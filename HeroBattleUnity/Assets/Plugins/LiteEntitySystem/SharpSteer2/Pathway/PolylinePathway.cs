// Copyright (c) 2002-2003, Sony Computer Entertainment America
// Copyright (c) 2002-2003, Craig Reynolds <craig_reynolds@playstation.sony.com>
// Copyright (C) 2007 Bjoern Graf <bjoern.graf@gmx.net>
// Copyright (C) 2007 Michael Coles <michael@digini.com>
// All rights reserved.
//
// This software is licensed as described in the file license.txt, which
// you should have received as part of this distribution. The terms
// are also available at http://www.codeplex.com/SharpSteer/Project/License.aspx.

using HeroBattle.FixedMath;
using System.Collections.Generic;
using System.Numerics;

namespace SharpSteer2.Pathway
{
	/// <summary>
	/// PolylinePathway: a simple implementation of the Pathway protocol.  The path
	/// is a "polyline" a series of line segments between specified points.  A
	/// radius defines a volume for the path which is the union of a sphere at each
	/// point and a cylinder along each segment.
	/// </summary>
	public class PolylinePathway : IPathway
	{
	    public int PointCount { get; private set; }

	    private readonly Vector3f[] _points;
	    public IEnumerable<Vector3f> Points
	    {
	        get
	        {
	            return _points;
	        }
	    }

	    public float Radius { get; private set; }
	    public bool Cyclic { get; private set; }

	    private readonly float[] _lengths;
	    private readonly Vector3f[] _tangents;

	    public float TotalPathLength { get; private set; }

		/// <summary>
        /// construct a PolylinePathway given the number of points (vertices),
        /// an array of points, and a path radius.
		/// </summary>
		/// <param name="points"></param>
		/// <param name="radius"></param>
		/// <param name="cyclic"></param>
        public PolylinePathway(IList<Vector3f> points, float radius, bool cyclic)
		{
            // set data members, allocate arrays
            Radius = radius;
            Cyclic = cyclic;
            PointCount = points.Count;
            TotalPathLength = 0;
            if (Cyclic)
                PointCount++;
            _lengths = new float[PointCount];
            _points = new Vector3f[PointCount];
            _tangents = new Vector3f[PointCount];

            // loop over all points
            for (int i = 0; i < PointCount; i++)
            {
                // copy in point locations, closing cycle when appropriate
                bool closeCycle = Cyclic && (i == PointCount - 1);
                int j = closeCycle ? 0 : i;
                _points[i] = points[j];

                // for the end of each segment
                if (i > 0)
                {
                    // compute the segment length
                    _tangents[i] = _points[i] - _points[i - 1];
                    _lengths[i] = _tangents[i].Length();

                    // find the normalized vector parallel to the segment
                    _tangents[i] *= 1 / _lengths[i];

                    // keep running total of segment lengths
                    TotalPathLength += _lengths[i];
                }
            }
		}

        public Vector3f MapPointToPath(Vector3f point, out Vector3f tangent, out float outside)
		{
            float minDistance = float.MaxValue;
            Vector3f onPath = Vector3f.Zero;
			tangent = Vector3f.Zero;

			// loop over all segments, find the one nearest to the given point
			for (int i = 1; i < PointCount; i++)
			{
			    Vector3f chosen;
			    float segmentProjection;
                float d = PointToSegmentDistance(point, _points[i - 1], _points[i], _tangents[i], _lengths[i], out chosen, out segmentProjection);
				if (d < minDistance)
				{
					minDistance = d;
                    onPath = chosen;
                    tangent = _tangents[i];
				}
			}

			// measure how far original point is outside the Pathway's "tube"
			outside = Vector3f.Distance(onPath, point) - Radius;

			// return point on path
			return onPath;
		}

        public float MapPointToPathDistance(Vector3f point)
		{
            float minDistance = float.MaxValue;
			float segmentLengthTotal = 0;
			float pathDistance = 0;

			for (int i = 1; i < PointCount; i++)
			{
			    Vector3f chosen;
			    float segmentProjection;
                float d = PointToSegmentDistance(point, _points[i - 1], _points[i], _tangents[i], _lengths[i], out chosen, out segmentProjection);
				if (d < minDistance)
				{
					minDistance = d;
                    pathDistance = segmentLengthTotal + segmentProjection;
				}
                segmentLengthTotal += _lengths[i];
			}

			// return distance along path of onPath point
			return pathDistance;
		}

        public Vector3f MapPathDistanceToPoint(float pathDistance)
		{
			// clip or wrap given path distance according to cyclic flag
			float remaining = pathDistance;
			if (Cyclic)
			{
				remaining = pathDistance % TotalPathLength;
			}
			else
			{
                if (pathDistance < 0) return _points[0];
                if (pathDistance >= TotalPathLength) return _points[PointCount - 1];
			}

			// step through segments, subtracting off segment lengths until
			// locating the segment that contains the original pathDistance.
			// Interpolate along that segment to find 3d point value to return.
			Vector3f result = Vector3f.Zero;
			for (int i = 1; i < PointCount; i++)
			{
                if (_lengths[i] < remaining)
				{
                    remaining -= _lengths[i];
				}
				else
				{
                    float ratio = remaining / _lengths[i];
                    result = Vector3f.Lerp(_points[i - 1], _points[i], ratio);
					break;
				}
			}
			return result;
		}

	    private static float PointToSegmentDistance(Vector3f point, Vector3f ep0, Vector3f ep1, Vector3f segmentTangent, float segmentLength, out Vector3f chosen, out float segmentProjection)
		{
			// convert the test point to be "local" to ep0
			Vector3f local = point - ep0;

			// find the projection of "local" onto "tangent"
            segmentProjection = Vector3f.Dot(segmentTangent, local);

			// handle boundary cases: when projection is not on segment, the
			// nearest point is one of the endpoints of the segment
			if (segmentProjection < 0)
			{
				chosen = ep0;
				segmentProjection = 0;
				return Vector3f.Distance(point, ep0);
			}
            if (segmentProjection > segmentLength)
			{
				chosen = ep1;
                segmentProjection = segmentLength;
				return Vector3f.Distance(point, ep1);
			}

			// otherwise nearest point is projection point on segment
            chosen = segmentTangent * segmentProjection;
			chosen += ep0;
			return Vector3f.Distance(point, chosen);
		}
	}
}
