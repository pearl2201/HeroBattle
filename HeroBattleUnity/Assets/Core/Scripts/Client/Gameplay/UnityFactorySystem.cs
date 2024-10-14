using HeroBattle;
using HeroBattleShare.Core;
using HeroBattleUnity.Gameplay;
using UnityEngine;

namespace HeroBattleShare.Factory
{
    public class UnityFactorySystem : Singleton<UnityFactorySystem>, IGameFactorySystem
    {
        public GameObject prefabBaseMinionView;
        public IBaseMinionView GetBaseMinionView(BaseMinion entity)
        {
            var unityObject = Instantiate(prefabBaseMinionView, entity.Position.ToVector3(), Quaternion.identity);
            unityObject.name = $"Projectile_{entity.Id}";
            var view = unityObject.GetComponent<ClientMinionView>();
            view.attached = entity;
            return view;
        }

        public void DestroyView(IEntityView entityView)
        {
            Destroy((GameObject)entityView.GetRoot());
        }
    }

}
