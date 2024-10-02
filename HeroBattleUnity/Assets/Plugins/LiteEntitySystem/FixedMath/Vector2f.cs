using FixMath.NET;
using LiteNetLib.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattle.FixedMath
{
    public struct Vector2f : INetSerializable
    {
        public Fix64 x; public Fix64 y;

        public Vector2f(Fix64 x, Fix64 y)
        {
            this.x = x;
            this.y = y;
        }

        public void Serialize(NetDataWriter writer)
        {
            writer.Put(x.RawValue);
            writer.Put(y.RawValue);
        }

        public void Deserialize(NetDataReader reader)
        {
            x = Fix64.FromRaw(reader.GetLong());
            y = Fix64.FromRaw(reader.GetLong());
        }

        public static Vector2f operator *(Vector2f x, Fix64 y)
        {
            return new Vector2f { x = x.x * y, y = x.y * y };
        }

        public static Vector2f operator +(Vector2f x, Vector2f y)
        {
            return new Vector2f { x = x.x + y.x, y = x.y + y.y };
        }
    }
}
