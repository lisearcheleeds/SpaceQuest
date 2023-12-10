using UnityEngine;

namespace AloneSpace.UI
{
    public class OverlayView : MonoBehaviour
    {
        [SerializeField] InventoryLogView inventoryLogView;
        [SerializeField] ContentQuickView contentQuickView;

        public void Initialize(QuestData questData)
        {
            inventoryLogView.Initialize();
            contentQuickView.Initialize();
        }

        public void Finalize()
        {
            inventoryLogView.Finalize();
            contentQuickView.OnUpdate();
        }

        public void OnUpdate()
        {
            inventoryLogView.OnUpdate();
            contentQuickView.OnUpdate();
        }
    }
}