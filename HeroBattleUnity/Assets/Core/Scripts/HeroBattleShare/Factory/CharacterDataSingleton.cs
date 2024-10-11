using HeroBattleShare.Characters;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattleShare.Factory
{
    public interface ICharacterRepositiory
    {
        BaseCharacterDefinition GetCharacterDefinition(int id);
    }
    public class CharacterDataSingleton : ICharacterRepositiory
    {
        public List<BaseCharacterDefinition> characterDefinitions;

        public BaseCharacterDefinition GetCharacterDefinition(int id)
        {
            return characterDefinitions.Find(x => x.id == id);
        }
    }
}
