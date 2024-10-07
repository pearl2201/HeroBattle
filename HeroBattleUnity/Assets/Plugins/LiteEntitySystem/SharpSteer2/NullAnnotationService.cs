
using HeroBattle.FixedMath;
using System.Numerics;

namespace SharpSteer2
{
    class NullAnnotationService
        :IAnnotationService
    {
        public bool IsEnabled
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        public void Line(Vector3f startPoint, Vector3f endPoint, Vector3f color, float opacity = 1)
        {

        }

        public void CircleXZ(float radius, Vector3f center, Vector3f color, int segments)
        {

        }

        public void DiskXZ(float radius, Vector3f center, Vector3f color, int segments)
        {

        }

        public void Circle3D(float radius, Vector3f center, Vector3f axis, Vector3f color, int segments)
        {

        }

        public void Disk3D(float radius, Vector3f center, Vector3f axis, Vector3f color, int segments)
        {

        }

        public void CircleOrDiskXZ(float radius, Vector3f center, Vector3f color, int segments, bool filled)
        {

        }

        public void CircleOrDisk3D(float radius, Vector3f center, Vector3f axis, Vector3f color, int segments, bool filled)
        {

        }

        public void CircleOrDisk(float radius, Vector3f axis, Vector3f center, Vector3f color, int segments, bool filled, bool in3D)
        {

        }

        public void AvoidObstacle(float minDistanceToCollision)
        {

        }

        public void PathFollowing(Vector3f future, Vector3f onPath, Vector3f target, float outside)
        {

        }

        public void AvoidCloseNeighbor(IVehicle other, float additionalDistance)
        {

        }

        public void AvoidNeighbor(IVehicle threat, float steer, Vector3f ourFuture, Vector3f threatFuture)
        {

        }

        public void VelocityAcceleration(IVehicle vehicle)
        {

        }

        public void VelocityAcceleration(IVehicle vehicle, float maxLength)
        {

        }

        public void VelocityAcceleration(IVehicle vehicle, float maxLengthAcceleration, float maxLengthVelocity)
        {

        }
    }
}
