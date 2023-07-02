using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] ActorView actorView;
        [SerializeField] TargetView targetView;
        [SerializeField] MenuView menuView;

        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] InteractionList interactionList;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;

        public void Initialize(QuestData questData)
        {
            actorView.Initialize();
            targetView.Initialize();
            menuView.Initialize(questData);

            mapPanelView.Initialize(questData);
            interactionList.Initialize();
            itemDataMenu.Initialize();
            inventoryView.Initialize();
        }

        public void Finalize()
        {
            actorView.Finalize();
            targetView.Finalize();
            menuView.Finalize();

            mapPanelView.Finalize();
            interactionList.Finalize();
            itemDataMenu.Finalize();
            inventoryView.Finalize();
        }

        public void OnUpdate()
        {
            actorView.OnUpdate();
            targetView.OnUpdate();
            menuView.OnUpdate();
        }
    }
}
