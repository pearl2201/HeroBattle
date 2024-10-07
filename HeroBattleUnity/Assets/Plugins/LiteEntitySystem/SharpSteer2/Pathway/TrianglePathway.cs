using System.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using SharpSteer2.Helpers;
using HeroBattle.FixedMath;

namespace SharpSteer2.Pathway
{
    /// <summary>
    /// A pathway made out of triangular segments
    /// </summary>
    public class TrianglePathway
        : IPathway
    {
        private readonly Triangle[] _path;

        public IEnumerable<Triangle> Triangles
        {
            get { return _path; }
        }

        private readonly PolylinePathway _centerline;
        public PolylinePathway Centerline
        {
            get
            {
                return _centerline;
            }
        }

        public TrianglePathway(IEnumerable<Triangle> path, bool cyclic = false)
        {
            _path = path.ToArray();

            //Calculate center points
            for (int i = 0; i < _path.Length; i++)
                _path[i].PointOnPath = (_path[i].A * 2 + _path[i].Edge0) / 2;

            //Calculate tangents along path
            for (int i = 0; i < _path.Length; i++)
            {
                var bIndex = cyclic ? ((i + 1) % _path.Length) : Math.Min(i + 1, _path.Length - 1);

                var vectorToNextTriangle = _path[bIndex].PointOnPath - _path[i].PointOnPath;
                var l = vectorToNextTriangle.Length();

                _path[i].Tangent = vectorToNextTriangle / l;

                if (Math.Abs(l) < float.Epsilon)
                    _path[i].Tangent = Vector3f.Zero;
            }

            _centerline = new PolylinePathway(_path.Select(a => a.PointOnPath).ToArray(), 0.1f, cyclic);
        }

        public Vector3f MapPointToPath(Vector3f point, out Vector3f tangent, out float outside)
        {
            int index;
            return MapPointToPath(point, out tangent, out outside, out index);
        }

        private Vector3f MapPointToPath(Vector3f point, out Vector3f tangent, out float outside, out int segmentIndex)
        {
            float distanceSqr = float.PositiveInfinity;
            Vector3f closestPoint = Vector3f.Zero;
            bool inside = false;
            segmentIndex = -1;

            for (int i = 0; i < _path.Length; i++)
            {
                bool isInside;
                var p = ClosestPointOnTriangle(ref _path[i], point, out isInside);

                var normal = (point - p);
                var dSqr = normal.LengthSquared();

                if (dSqr < distanceSqr)
                {
                    distanceSqr = dSqr;
                    closestPoint = p;
                    inside = isInside;
                    segmentIndex = i;
                }

                if (isInside)
                    break;
            }

            if (segmentIndex == -1)
                throw new InvalidOperationException("Closest Path Segment Not Found (Zero Length Path?");

            tangent = _path[segmentIndex].Tangent;
            outside = (float)Math.Sqrt(distanceSqr) * (inside ? -1 : 1);
            return closestPoint;
        }

        public Vector3f MapPathDistanceToPoint(float pathDistance)
        {
            return _centerline.MapPathDistanceToPoint(pathDistance);

            //// clip or wrap given path distance according to cyclic flag
            //if (_cyclic)
            //    pathDistance = pathDistance % _totalPathLength;
            //else
            //{
            //    if (pathDistance < 0)
            //        return _path[0].PointOnPath;
            //    if (pathDistance >= _totalPathLength)
            //        return _path[_path.Length - 1].PointOnPath;
            //}

            //// step through segments, subtracting off segment lengths until
            //// locating the segment that contains the original pathDistance.
            //// Interpolate along that segment to find 3d point value to return.
            //for (int i = 1; i < _path.Length; i++)
            //{
            //    if (_path[i].Length < pathDistance)
            //    {
            //        pathDistance -= _path[i].Length;
            //    }
            //    else
            //    {
            //        float ratio = pathDistance / _path[i].Length;

            //        var l = Vector3f.Lerp(_path[i].PointOnPath, _path[i].PointOnPath + _path[i].Tangent * _path[i].Length, ratio);
            //        return l;
            //    }
            //}

            //return Vector3f.Zero;
        }

        public float MapPointToPathDistance(Vector3f point)
        {
            return _centerline.MapPointToPathDistance(point);
        }

        public struct Triangle
        {
            public readonly Vector3f A;
            public readonly Vector3f Edge0;
            public readonly Vector3f Edge1;

            internal Vector3f Tangent;
            internal Vector3f PointOnPath;

            internal readonly float Determinant;

            public Triangle(Vector3f a, Vector3f b, Vector3f c)
            {
                A = a;
                Edge0 = b - a;
                Edge1 = c - a;

                PointOnPath = Vector3f.Zero;
                Tangent = Vector3f.Zero;

                // ReSharper disable once ImpureMethodCallOnReadonlyValueField
                var edge0LengthSquared = Edge0.LengthSquared();

                var edge0DotEdge1 = Vector3f.Dot(Edge0, Edge1);
                var edge1LengthSquared = Vector3f.Dot(Edge1, Edge1);

                Determinant = edge0LengthSquared * edge1LengthSquared - edge0DotEdge1 * edge0DotEdge1;
            }
        }

        private static Vector3f ClosestPointOnTriangle(ref Triangle triangle, Vector3f sourcePosition, out bool inside)
        {
            float a, b;
            return ClosestPointOnTriangle(ref triangle, sourcePosition, out a, out b, out inside);
        }

        internal static Vector3f ClosestPointOnTriangle(ref Triangle triangle, Vector3f sourcePosition, out float edge0Distance, out float edge1Distance, out bool inside)
        {
            Vector3f v0 = triangle.A - sourcePosition;

            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            float a = triangle.Edge0.LengthSquared();
            float b = Vector3f.Dot(triangle.Edge0, triangle.Edge1);
            // ReSharper disable once ImpureMethodCallOnReadonlyValueField
            float c = triangle.Edge1.LengthSquared();
            float d = Vector3f.Dot(triangle.Edge0, v0);
            float e = Vector3f.Dot(triangle.Edge1, v0);

            float det = triangle.Determinant;
            float s = b * e - c * d;
            float t = b * d - a * e;

            inside = false;
            if (s + t < det)
            {
                if (s < 0)
                {
                    if (t < 0)
                    {
                        if (d < 0)
                        {
                            s = Utilities.Clamp(-d / a, 0, 1);
                            t = 0;
                        }
                        else
                        {
                            s = 0;
                            t = Utilities.Clamp(-e / c, 0, 1);
                        }
                    }
                    else
                    {
                        s = 0;
                        t = Utilities.Clamp(-e / c, 0, 1);
                    }
                }
                else if (t < 0)
                {
                    s = Utilities.Clamp(-d / a, 0, 1);
                    t = 0;
                }
                else
                {
                    float invDet = 1 / det;
                    s *= invDet;
                    t *= invDet;
                    inside = true;
                }
            }
            else
            {
                if (s < 0)
                {
                    float tmp0 = b + d;
                    float tmp1 = c + e;
                    if (tmp1 > tmp0)
                    {
                        float numer = tmp1 - tmp0;
                        float denom = a - 2 * b + c;
                        s = Utilities.Clamp(numer / denom, 0, 1);
                        t = 1 - s;
                    }
                    else
                    {
                        t = Utilities.Clamp(-e / c, 0, 1);
                        s = 0;
                    }
                }
                else if (t < 0)
                {
                    if (a + d > b + e)
                    {
                        float numer = c + e - b - d;
                        float denom = a - 2 * b + c;
                        s = Utilities.Clamp(numer / denom, 0, 1);
                        t = 1 - s;
                    }
                    else
                    {
                        s = Utilities.Clamp(-e / c, 0, 1);
                        t = 0;
                    }
                }
                else
                {
                    float numer = c + e - b - d;
                    float denom = a - 2 * b + c;
                    s = Utilities.Clamp(numer / denom, 0, 1);
                    t = 1 - s;
                }
            }

            edge0Distance = s;
            edge1Distance = t;
            return triangle.A + triangle.Edge0 * s + triangle.Edge1 * t;
        }
    }
}
