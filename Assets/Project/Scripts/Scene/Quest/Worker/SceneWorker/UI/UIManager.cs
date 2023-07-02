using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [Header("Instruments")]
        [SerializeField] ActorView actorView;
        [SerializeField] TargetView targetView;
        // [SerializeField] CockpitView cockpitView;

        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] InteractionList interactionList;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;

        public void Initialize(QuestData questData)
        {
            actorView.Initialize();
            targetView.Initialize();

            mapPanelView.Initialize(questData);
            interactionList.Initialize();
            itemDataMenu.Initialize();
            inventoryView.Initialize();
        }

        public void Finalize()
        {
            actorView.Finalize();
            targetView.Finalize();

            mapPanelView.Finalize();
            interactionList.Finalize();
            itemDataMenu.Finalize();
            inventoryView.Finalize();
        }

        public void OnUpdate()
        {
            actorView.OnUpdate();
            targetView.OnUpdate();
        }
    }
}
