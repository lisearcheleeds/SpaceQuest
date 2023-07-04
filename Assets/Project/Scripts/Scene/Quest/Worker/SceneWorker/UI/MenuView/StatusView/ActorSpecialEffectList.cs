using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class ActorSpecialEffectList : MonoBehaviour
    {
        ActorData actorData;

        bool isDirty;

        public void Initialize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.AddListener(UIMenuStatusViewSelectActorData);
        }

        public void Finalize()
        {
            MessageBus.Instance.UIMenuStatusViewSelectActorData.RemoveListener(UIMenuStatusViewSelectActorData);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }
        }

        void Refresh()
        {
        }

        void UIMenuStatusViewSelectActorData(ActorData actorData)
        {
            this.actorData = actorData;
            isDirty = true;
        }
    }
}
