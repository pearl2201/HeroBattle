using HeroBattle;
using HeroBattleShare;
using HeroBattleShare.Core;
using HeroBattleShare.Factory;
using UnityEngine;

namespace HeroBattleUnity.Gameplay
{
    public class ClientEntityView : MonoBehaviour, IEntityView
    {
        public object GetRoot()
        {
            return this.gameObject;
        }
    }
    public class ClientMinionView : ClientEntityView, IBaseMinionView
    {
        public BaseMinion attached { get; set; }

        void Update()
        {
            if (attached != null)
            {
                this.transform.position = attached.position.Value.ToVector3();
            }
        }

    }


}
