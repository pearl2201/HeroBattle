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

        public static Vector2f operator *(Vector2f x, float y)
        {
            return new Vector2f { x = x.x * y, y = x.y * y };
        }

        public static Vector2f operator +(Vector2f x, Vector2f y)
        {
            return new Vector2f { x = x.x + y.x, y = x.y + y.y };
        }
    }
}
