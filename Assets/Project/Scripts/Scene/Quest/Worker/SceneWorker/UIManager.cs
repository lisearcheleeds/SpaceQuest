using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [Header("Instruments")]
        [SerializeField] WeaponDataListView weaponDataListView;
        [SerializeField] TargetView targetView;
        // [SerializeField] CockpitView cockpitView;

        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] CameraAngleController cameraAngleController;
        [SerializeField] InteractionList interactionList;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;

        [Header("3D")]
        [SerializeField] CameraAngleControllerEffect cameraAngleControllerEffect;

        UserData userData;

        public void Initialize(QuestData questData)
        {
            this.userData = questData.UserData;

            cameraAngleControllerEffect.Initialize();

            weaponDataListView.Initialize();
            targetView.Initialize();

            mapPanelView.Initialize(questData);
            cameraAngleController.Initialize();
            interactionList.Initialize();
            itemDataMenu.Initialize();
            inventoryView.Initialize();
        }

        public void Finalize()
        {
            cameraAngleControllerEffect.Finalize();

            weaponDataListView.Finalize();
            targetView.Finalize();

            mapPanelView.Finalize();
            cameraAngleController.Finalize();
            interactionList.Finalize();
            itemDataMenu.Finalize();
            inventoryView.Finalize();
        }

        public void OnLateUpdate()
        {
            weaponDataListView.OnLateUpdate();
            targetView.OnLateUpdate();
        }
    }
}
