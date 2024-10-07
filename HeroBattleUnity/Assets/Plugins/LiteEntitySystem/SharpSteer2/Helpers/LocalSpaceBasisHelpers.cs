using HeroBattle.FixedMath;
using System.Numerics;

namespace SharpSteer2.Helpers
{
    public static class LocalSpaceBasisHelpers
    {
        /// <summary>
        /// Transforms a direction in global space to its equivalent in local space.
        /// </summary>
        /// <param name="basis">The basis which this should operate on</param>
        /// <param name="globalDirection">The global space direction to transform.</param>
        /// <returns>The global space direction transformed to local space .</returns>
        public static Vector3f LocalizeDirection(this ILocalSpaceBasis basis, Vector3f globalDirection)
        {
            // dot offset with local basis vectors to obtain local coordiantes
            return new Vector3f(Vector3f.Dot(globalDirection, basis.Side), Vector3f.Dot(globalDirection, basis.Up), Vector3f.Dot(globalDirection, basis.Forward));
        }

        /// <summary>
        /// Transforms a point in global space to its equivalent in local space.
        /// </summary>
        /// <param name="basis">The basis which this should operate on</param>
        /// <param name="globalPosition">The global space position to transform.</param>
        /// <returns>The global space position transformed to local space.</returns>
        public static Vector3f LocalizePosition(this ILocalSpaceBasis basis, Vector3f globalPosition)
        {
            // global offset from local origin
            Vector3f globalOffset = globalPosition - basis.Position;

            // dot offset with local basis vectors to obtain local coordiantes
            return LocalizeDirection(basis, globalOffset);
        }

        /// <summary>
        /// Transforms a point in local space to its equivalent in global space.
        /// </summary>
        /// <param name="basis">The basis which this should operate on</param>
        /// <param name="localPosition">The local space position to tranform.</param>
        /// <returns>The local space position transformed to global space.</returns>
        public static Vector3f GlobalizePosition(this ILocalSpaceBasis basis, Vector3f localPosition)
        {
            return basis.Position + GlobalizeDirection(basis, localPosition);
        }

        /// <summary>
        /// Transforms a direction in local space to its equivalent in global space.
        /// </summary>
        /// <param name="basis">The basis which this should operate on</param>
        /// <param name="localDirection">The local space direction to tranform.</param>
        /// <returns>The local space direction transformed to global space</returns>
        public static Vector3f GlobalizeDirection(this ILocalSpaceBasis basis, Vector3f localDirection)
        {
            return ((basis.Side * localDirection.X) +
                    (basis.Up * localDirection.Y) +
                    (basis.Forward * localDirection.Z));
        }

        /// <summary>
        /// Rotates, in the canonical direction, a vector pointing in the
        /// "forward" (+Z) direction to the "side" (+/-X) direction as implied
        /// by IsRightHanded.
        /// </summary>
        /// <param name="basis">The basis which this should operate on</param>
        /// <param name="value">The local space vector.</param>
        /// <returns>The rotated vector.</returns>
        public static Vector3f LocalRotateForwardToSide(this ILocalSpaceBasis basis, Vector3f value)
        {
            return new Vector3f(-value.Z, value.Y, value.X);
        }

        public static void ResetLocalSpace(out Vector3f forward, out Vector3f side, out Vector3f up, out Vector3f position)
        {
            forward = -Vector3f.UnitZ;
            side = Vector3f.UnitX;
            up = Vector3f.UnitY;
            position = Vector3f.Zero;
        }

        /// <summary>
        /// set "side" basis vector to normalized cross product of forward and up
        /// </summary>
        /// <param name="forward"></param>
        /// <param name="side"></param>
        /// <param name="up"></param>
        public static void SetUnitSideFromForwardAndUp(ref Vector3f forward, out Vector3f side, ref Vector3f up)
        {
            // derive new unit side basis vector from forward and up
            side = Vector3f.Normalize(Vector3f.Cross(forward, up));
        }

        /// <summary>
        /// regenerate the orthonormal basis vectors given a new forward
        /// (which is expected to have unit length)
        /// </summary>
        /// <param name="newUnitForward"></param>
        /// <param name="forward"></param>
        /// <param name="side"></param>
        /// <param name="up"></param>
        public static void RegenerateOrthonormalBasisUF(Vector3f newUnitForward, out Vector3f forward, out Vector3f side, ref Vector3f up)
        {
            forward = newUnitForward;

            // derive new side basis vector from NEW forward and OLD up
            SetUnitSideFromForwardAndUp(ref forward, out side, ref up);

            // derive new Up basis vector from new Side and new Forward
            //(should have unit length since Side and Forward are
            // perpendicular and unit length)
            up = Vector3f.Cross(side, forward);
        }

        /// <summary>
        /// for when the new forward is NOT know to have unit length
        /// </summary>
        /// <param name="newForward"></param>
        /// <param name="forward"></param>
        /// <param name="side"></param>
        /// <param name="up"></param>
        public static void RegenerateOrthonormalBasis(Vector3f newForward, out Vector3f forward, out Vector3f side, ref Vector3f up)
        {
            RegenerateOrthonormalBasisUF(Vector3f.Normalize(newForward), out forward, out side, ref up);
        }

        /// <summary>
        /// for supplying both a new forward and and new up
        /// </summary>
        /// <param name="newForward"></param>
        /// <param name="newUp"></param>
        /// <param name="forward"></param>
        /// <param name="side"></param>
        /// <param name="up"></param>
        public static void RegenerateOrthonormalBasis(Vector3f newForward, Vector3f newUp, out Vector3f forward, out Vector3f side, out Vector3f up)
        {
            up = newUp;
            RegenerateOrthonormalBasis(Vector3f.Normalize(newForward), out forward, out side, ref up);
        }

        public static Matrix4x4 ToMatrix(this ILocalSpaceBasis basis)
        {
            return ToMatrix(basis.Forward, basis.Side, basis.Up, basis.Position);
        }

        public static Matrix4x4 ToMatrix(Vector3f forward, Vector3f side, Vector3f up, Vector3f position)
        {
            Matrix4x4 m = Matrix4x4.Identity;
            m.Translation = position;
            MatrixHelpers.Right(ref m, ref side);
            MatrixHelpers.Up(ref m, ref up);
            MatrixHelpers.Right(ref m, ref forward);

            return m;
        }

        public static void FromMatrix(Matrix4x4 transformation, out Vector3f forward, out Vector3f side, out Vector3f up, out Vector3f position)
        {
            position = transformation.Translation;
            side = MatrixHelpers.Right(ref transformation);
            up = MatrixHelpers.Up(ref transformation);
            forward = MatrixHelpers.Backward(ref transformation);
        }
    }
}
