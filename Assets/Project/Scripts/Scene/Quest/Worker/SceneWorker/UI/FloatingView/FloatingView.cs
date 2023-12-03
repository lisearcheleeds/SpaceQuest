using UnityEngine;

namespace AloneSpace
{
    public class FloatingView : MonoBehaviour
    {
        [SerializeField] TargetView targetView;
        [SerializeField] RadarView radarView;
        [SerializeField] InventoryLogView inventoryLogView;

        public void Initialize(QuestData questData)
        {
            targetView.Initialize();
            radarView.Initialize(questData);
            inventoryLogView.Initialize();
        }

        public void Finalize()
        {
            targetView.Finalize();
            radarView.Finalize();
            inventoryLogView.Finalize();
        }

        public void OnUpdate()
        {
            targetView.OnUpdate();
            radarView.OnUpdate();
            inventoryLogView.OnUpdate();
        }
    }
}