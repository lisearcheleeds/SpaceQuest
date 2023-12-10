using UnityEngine;

namespace AloneSpace.UI
{
    public class FloatView : MonoBehaviour
    {
        [SerializeField] TargetView targetView;
        [SerializeField] RadarView radarView;

        public void Initialize(QuestData questData)
        {
            targetView.Initialize();
            radarView.Initialize(questData);
        }

        public void Finalize()
        {
            targetView.Finalize();
            radarView.Finalize();
        }

        public void OnUpdate()
        {
            targetView.OnUpdate();
            radarView.OnUpdate();
        }
    }
}