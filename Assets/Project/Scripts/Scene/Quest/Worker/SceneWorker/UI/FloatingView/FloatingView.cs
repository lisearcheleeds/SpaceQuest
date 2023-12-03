using UnityEngine;

namespace AloneSpace
{
    public class FloatingView : MonoBehaviour
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