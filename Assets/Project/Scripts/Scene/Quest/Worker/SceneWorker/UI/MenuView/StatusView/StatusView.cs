using UnityEngine;

namespace AloneSpace
{
    public class StatusView : MonoBehaviour
    {
        [SerializeField] ActorList actorList;
        [SerializeField] ActorStatusView actorStatusView;
        [SerializeField] ActorSpecView actorSpecView;

        public void Initialize(QuestData questData)
        {
            actorList.Initialize(questData);
            actorStatusView.Initialize();
            actorSpecView.Initialize();
        }

        public void Finalize()
        {
            actorList.Finalize();
            actorStatusView.Finalize();
            actorSpecView.Finalize();
        }

        public void OnUpdate()
        {
            actorList.OnUpdate();
            actorStatusView.OnUpdate();
            actorSpecView.OnUpdate();
        }
    }
}
