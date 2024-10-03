﻿using HeroBattle;
using HeroBattleShare.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroBattleShare
{
    public interface IBaseMinionView : IEntityView
    {
        BaseMinion attached { get; set; }
    }
}
