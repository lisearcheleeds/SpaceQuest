using UnityEngine;

namespace AloneSpace.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] FloatView floatView;
        [SerializeField] FixedView fixedView;
        [SerializeField] OverlayView overlayView;

        public void Initialize(QuestData questData)
        {
            floatView.Initialize(questData);
            fixedView.Initialize(questData);
            overlayView.Initialize(questData);
        }

        public void Finalize()
        {
            floatView.Finalize();
            fixedView.Finalize();
            overlayView.Finalize();
        }

        public void OnUpdate()
        {
            floatView.OnUpdate();
            fixedView.OnUpdate();
            overlayView.OnUpdate();
        }
    }
}
