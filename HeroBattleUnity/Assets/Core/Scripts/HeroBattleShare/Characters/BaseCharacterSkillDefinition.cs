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
        public List<BaseCharacterSkillLevelDefinition> levels;

        public abstract void OnVisited(BaseMinion minion);
    }
}
