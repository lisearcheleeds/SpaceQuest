using UnityEngine;

namespace AloneSpace.UI
{
    public class FloatView : MonoBehaviour
    {
        [SerializeField] TargetView targetView;
        [SerializeField] RadarView radarView;
        [SerializeField] ReticleView reticleView;

        public void Initialize(QuestData questData)
        {
            targetView.Initialize();
            radarView.Initialize(questData);
            reticleView.Initialize(questData);
        }

        public void Finalize()
        {
            targetView.Finalize();
            radarView.Finalize();
            reticleView.Finalize();
        }

        public void OnUpdate()
        {
            targetView.OnUpdate();
            radarView.OnUpdate();
            reticleView.OnUpdate();
        }
    }
}