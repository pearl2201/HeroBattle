using System.Numerics;
using System.Collections.Generic;
using HeroBattle.FixedMath;

namespace SharpSteer2.Pathway
{
    /// <summary>
    /// A path consisting of a series of gates which must be passed through
    /// </summary>
    public class GatewayPathway
        : IPathway
    {
        public PolylinePathway Centerline
        {
            get
            {
                return _trianglePathway.Centerline;
            }
        }

        private readonly TrianglePathway _trianglePathway;
        public TrianglePathway TrianglePathway
        {
            get
            {
                return _trianglePathway;
            }
        }

        public GatewayPathway(IEnumerable<Gateway> gateways, bool cyclic = false)
        {
            List<TrianglePathway.Triangle> triangles = new List<TrianglePathway.Triangle>();

            bool first = true;
            Gateway previous = default(Gateway);
            Vector3f previousNormalized = Vector3f.Zero;
            foreach (var gateway in gateways)
            {
                var n = Vector3f.Normalize(gateway.B - gateway.A);

                if (!first)
                {
                    if (Vector3f.Dot(n, previousNormalized) < 0)
                    {
                        triangles.Add(new TrianglePathway.Triangle(previous.A, previous.B, gateway.A));
                        triangles.Add(new TrianglePathway.Triangle(previous.A, gateway.A, gateway.B));
                    }
                    else
                    {
                        triangles.Add(new TrianglePathway.Triangle(previous.A, previous.B, gateway.A));
                        triangles.Add(new TrianglePathway.Triangle(previous.B, gateway.A, gateway.B));
                    }
                }
                first = false;

                previousNormalized = n;
                previous = gateway;
            }

            _trianglePathway = new TrianglePathway(triangles, cyclic);

        }

        public struct Gateway
        {
            public readonly Vector3f A;
            public readonly Vector3f B;

            public Gateway(Vector3f a, Vector3f b)
                : this()
            {
                A = a;
                B = b;
            }
        }

        public Vector3f MapPointToPath(Vector3f point, out Vector3f tangent, out float outside)
        {
            return _trianglePathway.MapPointToPath(point, out tangent, out outside);
        }

        public Vector3f MapPathDistanceToPoint(float pathDistance)
        {
            return _trianglePathway.MapPathDistanceToPoint(pathDistance);
        }

        public float MapPointToPathDistance(Vector3f point)
        {
            return _trianglePathway.MapPointToPathDistance(point);
        }
    }
}
