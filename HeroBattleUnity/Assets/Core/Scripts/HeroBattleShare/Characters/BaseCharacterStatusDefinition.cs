using HeroBattle;
using System;
using System.Collections.Generic;
using System.Text;

namespace HeroBattleShare.Characters
{
    public abstract class BaseCharacterStatusDefinition
    {
        public int turn;
        public abstract void OnPlayerReceiveHp(BaseMinion minion);
        public abstract void OnPlayerReceiveSp(BaseMinion minion);

        public abstract void OnPlayerUpdate(BaseMinion minion);

        public abstract void OnPlayerReceiveStatus(BaseMinion minion);

        public abstract void OnPlayerEndStatus(BaseMinion minion);
    }
}
