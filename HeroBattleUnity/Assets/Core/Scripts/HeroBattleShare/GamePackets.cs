using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace HeroBattle
{
    public class GamePackets
    {
        public enum PacketType : byte
        {
            EntitySystem,
            Serialized
        }

        //Auto serializable packets
        public class JoinPacket
        {
            public string UserName { get; set; }
        }

        [Flags]
        public enum MovementKeys : byte
        {
            Left = 1,
            Right = 1 << 1,
            Up = 1 << 2,
            Down = 1 << 3,
            Fire = 1 << 4,
            Projectile = 1 << 5
        }

        public struct ShootPacket
        {
            public Vector2 Origin;
            public Vector2 Hit;
        }

        public struct PlayerInputPacket
        {
            public MovementKeys Keys;
            public float Rotation;
        }

        public struct AttackPacket{

        }

        public struct HitPacket{
            
        }
    }
}
