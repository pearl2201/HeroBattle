using HeroBattle;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattleShare.Characters
{
    // Visitor pattern
    public abstract class BaseCharacterSkillDefinition
    {
        public string name;
        public int mana;
        public int level;
        public SkillDamageType attackRange;
        public EffectStatus effects;

        public int projectileRange;
        public int projectileDamage;
        public int projectileSpeed;

        public abstract void OnVisited(BaseMinion minion);
    }
}
