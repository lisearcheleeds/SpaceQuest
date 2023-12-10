using UnityEngine;

namespace AloneSpace.UI
{
    public class ActorOperationModeView : MonoBehaviour
    {
        [SerializeField] GameObject[] actorOperationModeIcons;

        ActorData userControlActor;
        bool isDirty;

        public void Initialize()
        {
            MessageBus.Instance.UserCommandSetActorOperationMode.AddListener(UserCommandSetActorOperationMode);

            foreach (var actorOperationModeIcon in actorOperationModeIcons)
            {
                actorOperationModeIcon.SetActive(false);
            }
        }

        public void Finalize()
        {
            MessageBus.Instance.UserCommandSetActorOperationMode.RemoveListener(UserCommandSetActorOperationMode);
        }

        public void OnUpdate()
        {
        }

        void UserCommandSetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            var index = GetActorOperationModeIconsIndex(actorOperationMode);
            for (var i = 0; i < actorOperationModeIcons.Length; i++)
            {
                actorOperationModeIcons[i].SetActive(index == i);
            }
        }

        int GetActorOperationModeIconsIndex(ActorOperationMode actorOperationMode)
        {
            return actorOperationMode switch
            {
                ActorOperationMode.Observe => 0,
                ActorOperationMode.Cockpit => 1,
                ActorOperationMode.CockpitFreeCamera => 2,
                ActorOperationMode.Spotter => 3,
                ActorOperationMode.SpotterFreeCamera => 4,
            };
        }
    }
}
