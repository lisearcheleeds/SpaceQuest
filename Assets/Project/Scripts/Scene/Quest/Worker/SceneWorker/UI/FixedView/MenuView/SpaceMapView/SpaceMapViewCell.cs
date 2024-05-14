using System;
using AloneSpace.Common;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class SpaceMapViewCell : MonoBehaviour
    {
        [SerializeField] Circle2D circle2D;
        [SerializeField] Button button;

        ActorData actorData;
        Action<ActorData> onClickActorData;

        IInteractData interactData;
        Action<IInteractData> onClickInteractData;

        public void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        public void Apply(ActorData actorData, Vector3 position, Action<ActorData> onClick)
        {
            this.actorData = actorData;
            this.onClickActorData = onClick;
            this.interactData = null;
            this.onClickInteractData = null;

            transform.localPosition = position;
            circle2D.color = GetColor();
        }

        public void Apply(IInteractData interactData, Vector3 position, Action<IInteractData> onClick)
        {
            this.actorData = null;
            this.onClickActorData = null;
            this.interactData = interactData;
            this.onClickInteractData = onClick;

            transform.localPosition = position;
            circle2D.color = GetColor();
        }

        void OnClick()
        {
            if (actorData != null)
            {
                onClickActorData(actorData);
            }
            else
            {
                onClickInteractData(interactData);
            }
        }

        static Color GetColor()
        {
            return Color.white;
        }
    }
}
