using HeroBattle.FixedMath;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace SharpSteer2.Helpers
{
    public static class MatrixHelpers
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Right(ref Matrix4x4 m, ref Vector3f v)
        {
            m.M11 = v.X;
            m.M12 = v.Y;
            m.M13 = v.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Right(ref Matrix4x4 m)
        {
            return new Vector3f {
                X = m.M11,
                Y = m.M12,
                Z = m.M13
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Up(ref Matrix4x4 m, ref Vector3f v)
        {
            m.M21 = v.X;
            m.M22 = v.Y;
            m.M23 = v.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Up(ref Matrix4x4 m)
        {
            return new Vector3f {
                X = m.M21,
                Y = m.M22,
                Z = m.M23
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Backward(ref Matrix4x4 m, ref Vector3f v)
        {
            m.M31 = v.X;
            m.M32 = v.Y;
            m.M33 = v.Z;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector3f Backward(ref Matrix4x4 m)
        {
            return new Vector3f {
                X = m.M31,
                Y = m.M32,
                Z = m.M33
            };
        }
    }
}
