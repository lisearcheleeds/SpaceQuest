using UnityEngine;

namespace AloneSpace
{
    public class MenuView : MonoBehaviour
    {
        [SerializeField] StatusView statusView;

        public void Initialize(QuestData questData)
        {
            statusView.Initialize(questData);
        }

        public void Finalize()
        {
            statusView.Finalize();
        }

        public void OnUpdate()
        {
            statusView.OnUpdate();
        }
    }
}
