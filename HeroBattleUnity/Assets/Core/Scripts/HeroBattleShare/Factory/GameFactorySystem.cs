using HeroBattle;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroBattleShare.Factory
{
    public interface IGameFactorySystem
    {
        IBaseMinionView GetBaseMinionView(BaseMinion entity);

        void DestroyView(IEntityView entityView);
    }

    public interface IEntityView
    {
        public object GetRoot();
    }
}
