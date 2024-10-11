using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace HeroBattleShare.Characters
{
    public class BaseCharacterSkillLevelDefinition
    {
        public int mana;
        public SkillDamageType attackRange;
        public EffectStatus effects;

        public int projectileRange;
        public int projectileDamage;
        public int projectileSpeed;

    }
}
