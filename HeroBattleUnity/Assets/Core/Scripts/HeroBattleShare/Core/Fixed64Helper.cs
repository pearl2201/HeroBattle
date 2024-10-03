#if UNITY_2017_1_OR_NEWER
using HeroBattle.FixedMath;
using UnityEngine;

namespace HeroBattleShare.Core
{
    public static class Fixed64Helper
    {
        public static Vector3 ToVector3(this Vector2f v)
        {
            return new Vector3((float)v.x, (float)v.y, 0);
        }
    }
}
#endif
