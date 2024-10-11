using System;
using System.Collections.Generic;

namespace HeroBattleShare.Characters
{
    [Flags]
    public enum EffectStatus : Int64
    {
        None = 0,
        Slow = 1 << 0,
        Freeze = 2 << 0,

    }

    public enum SkillTarget
    {
        Enemy = 0,
        Team = 1,
    }

    public enum SkillDamageType
    {
        Single,
        Aoe
    }
    public class BaseCharacterDefinition
    {
        public int id;
        public string name;
        public List<BaseCharacterStatDefinition> StatDefinitions = new List<BaseCharacterStatDefinition>();
        public List<BaseCharacterSkillDefinition> SkillDefinitions = new List<BaseCharacterSkillDefinition>();
    }
}
