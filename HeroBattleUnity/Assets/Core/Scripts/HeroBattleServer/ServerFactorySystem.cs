using HeroBattle;
using HeroBattleShare;
using HeroBattleShare.Factory;

namespace HeroBattleServer.Factory
{
    public class ServerFactorySystem : IGameFactorySystem
    {
        public IBaseMinionView GetBaseMinionView(BaseMinion entity)
        {
            return new ServerBaseMinionView();
        }

        public void DestroyView(IEntityView entityView)
        {
            var root = entityView.GetRoot();
            root = null;
        }
    }

    public class ServerBaseEntityView : IEntityView
    {
        public object GetRoot()
        {
            return this;
        }
    }

    public class ServerBaseMinionView : ServerBaseEntityView, IBaseMinionView
    {
        public BaseMinion attached { get; set; }
    }

}
