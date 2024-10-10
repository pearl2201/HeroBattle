using FixMath.NET;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattle.FixedMath
{
    public struct Vector2f : INetSerializable
    {
        public float x; public float y;

        public Vector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
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

        public static Vector2f operator *(Vector2f a, float b)
        {
            return new Vector2f(a.x * b, a.y * b);
        }

        public static Vector2f operator *(float b, Vector2f a)
        {
            return new Vector2f(a.x * b, a.y * b);
        }

        public static Vector2f operator -(Vector2f a)
        {
            return new Vector2f(-a.x, -a.y);
        }

        public static Vector2f operator /(Vector2f a, float b)
        {
            return new Vector2f(a.x / b, a.y / b);
        }

        public static Vector2f operator +(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x + b.x, a.y + b.y);
        }

        public static Vector2f operator -(Vector2f a, Vector2f b)
        {
            return new Vector2f(a.x - b.x, a.y - b.y);
        }

        public static bool operator ==(Vector2f a, Vector2f b)
        {
            return a.x == b.x && a.y == b.y;
        }

        public static bool operator !=(Vector2f a, Vector2f b)
        {
            return a.x != b.x || a.y != b.y;
        }

        public float Length()
        {
            return (float)Math.Sqrt(LengthSquared());
        }

        public float LengthSquared()
        {
            return x * x + y * y;
        }
        public static Vector2f Zero = new Vector2f(0, 0);
        public static Vector2f One = new Vector2f(1, 1);

        public static Vector2f Multiply(Vector2f a, float v)
        {
            return a * v;
        }
        public static Vector2f Normalize(Vector2f v)
        {
            return v / v.Length();
        }

        public static float Distance(Vector2f a, Vector2f b)
        {
            return (a - b).Length();
        }

        public static float Dot(Vector2f a, Vector2f b)
        {
            return a.x * b.x + a.y * b.y;
        }




        public static Vector2f Lerp(Vector2f a, Vector2f b, float p)
        {
            return a + (b - a) * p;
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2f f &&
                   x == f.x &&
                   y == f.y;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y);
        }

        public static Vector2f UnitX = new Vector2f(1, 0);
        public static Vector2f UnitY = new Vector2f(0, 1);

        public bool Equals(Vector2f f)
        {
            return
                x == f.x &&
                y == f.y;
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"x: {x}, y: {y}";
        }
    }
}
