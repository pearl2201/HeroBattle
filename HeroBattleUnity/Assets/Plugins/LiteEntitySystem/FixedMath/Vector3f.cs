using LiteNetLib.Utils;
using System;

namespace HeroBattle.FixedMath
{
    public struct Vector3f : INetSerializable, IEquatable<Vector3f>, IFormattable
    {
        public float x;
        public float y;
        public float z;

        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }
        public float Z { get { return z; } set { z = value; } }

        public Vector3f(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }


        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x);
            writer.Put(y);
        }

        public void Deserialize(NetDataReader reader)
        {
            x = reader.GetFloat();
            y = reader.GetFloat();
        }

        public static Vector3f operator *(Vector3f a, float b)
        {
            return new Vector3f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator *(float b, Vector3f a)
        {
            return new Vector3f(a.x * b, a.y * b, a.z * b);
        }

        public static Vector3f operator -(Vector3f a)
        {
            return new Vector3f(-a.x, -a.y, -a.z);
        }

        public static Vector3f operator /(Vector3f a, float b)
        {
            return new Vector3f(a.x / b, a.y / b, a.z / b);
        }

        public static Vector3f operator +(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x + b.x, a.y + b.y, a.z + b.z);
        }

        public static Vector3f operator -(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.x - b.x, a.y - b.y, a.z - b.z);
        }

        public static bool operator ==(Vector3f a, Vector3f b)
        {
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }

        public static bool operator !=(Vector3f a, Vector3f b)
        {
            return a.x != b.x || a.y != b.y || a.z != b.z;
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return x * x + y * y + z * z;
        }
        public static Vector3f Zero = new Vector3f(0, 0, 0);
        public static Vector3f One = new Vector3f(1, 1, 1);

        public static Vector3f Multiply(Vector3f a, float v)
        {
            return a * v;
        }
        public static Vector3f Normalize(Vector3f v)
        {
            return v / v.Length();
        }

        public static float Distance(Vector3f a, Vector3f b)
        {
            return (a - b).Length();
        }

        public static float Dot(Vector3f a, Vector3f b)
        {
            return a.x * b.x + a.y * b.y + a.z * b.z;
        }

        public static Vector3f Cross(Vector3f a, Vector3f b)
        {
            return new Vector3f(a.y * b.z - a.z * b.y, a.z * b.x - a.x * b.z, a.x * b.y - a.y * b.x);
        }


        public static Vector3f Lerp(Vector3f a, Vector3f b, float p)
        {
            return a + (b - a) * p;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3f f &&
                   x == f.x &&
                   y == f.y &&
                   z == f.z;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public static Vector3f UnitX = new Vector3f(1, 0, 0);
        public static Vector3f UnitY = new Vector3f(0, 1, 0);
        public static Vector3f UnitZ = new Vector3f(0, 0, 1);

        public bool Equals(Vector3f f)
        {
            return
                x == f.x &&
                y == f.y &&
                z == f.z;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"x: {x}, y: {y},z: {z}";
        }
    }
}
