using UnityEngine;

namespace AloneSpace.UI
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] ActorList actorList;
        [SerializeField] ActorStatusView actorStatusView;
        [SerializeField] ActorSpecView actorSpecView;
        [SerializeField] ActorSpecialEffectList actorSpecialEffectList;

        public void Initialize(QuestData questData)
        {
            actorList.Initialize(questData);
            actorStatusView.Initialize();
            actorSpecView.Initialize();
            actorSpecialEffectList.Initialize();
        }

        public void Finalize()
        {
            actorList.Finalize();
            actorStatusView.Finalize();
            actorSpecView.Finalize();
            actorSpecialEffectList.Finalize();
        }

        public void SetDirty()
        {
            actorList.SetDirty();
            actorStatusView.SetDirty();
            actorSpecView.SetDirty();
            actorSpecialEffectList.SetDirty();
        }

        public void OnUpdate()
        {
            actorList.OnUpdate();
            actorStatusView.OnUpdate();
            actorSpecView.OnUpdate();
            actorSpecialEffectList.OnUpdate();
        }
    }
}
