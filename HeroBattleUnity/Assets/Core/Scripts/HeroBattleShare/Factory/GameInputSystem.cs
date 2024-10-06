using HeroBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FixMath.NET;
using HeroBattle.FixedMath;

namespace HeroBattleShare.Factory
{
    public interface IGameInputSystem
    {
        Vector2f GetPlayerInput();
    }
}
