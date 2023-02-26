using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace AloneSpace
{
    public class UIManager : MonoBehaviour
    {
        [Header("Buttons")]
        [SerializeField] Button mapButton;
        [SerializeField] Button interactButton;
        [SerializeField] Button inventoryButton;
        [SerializeField] Button switchActorModeCombatButton;
        
        [Header("Center")]
        [SerializeField] MapPanelView mapPanelView;
        [SerializeField] CameraAngleController cameraAngleController;
        [SerializeField] InteractionList interactionList;
        [SerializeField] ItemDataMenu itemDataMenu;
        [SerializeField] InventoryView inventoryView;
        
        [Header("3D")]
        [SerializeField] CameraAngleControllerEffect cameraAngleControllerEffect;

        PlayerQuestData observePlayerQuestData;
        
        public void Initialize(QuestData questData)
        {
            cameraAngleControllerEffect.Initialize();
            
            mapPanelView.Initialize(questData);
            cameraAngleController.Initialize();
            interactionList.Initialize(questData);
            itemDataMenu.Initialize();
            inventoryView.Initialize(questData);
            
            mapButton.onClick.AddListener(OnClickMap);
            interactButton.onClick.AddListener(OnClickInteract);
            inventoryButton.onClick.AddListener(OnClickInventory);
            switchActorModeCombatButton.onClick.AddListener(OnClickSwitchActorModeCombatButton);
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate()
        {
            // WASDとマウス
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit)
            {
                MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(Keyboard.current.wKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputFrontBoosterPowerRatio.Broadcast(Keyboard.current.sKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(Keyboard.current.aKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(Keyboard.current.dKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(Keyboard.current.leftCtrlKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(Keyboard.current.spaceKey.isPressed ? 1.0f : 0.0f);

                var mouseDelta = Mouse.current.delta.ReadValue();
                MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(mouseDelta.x);
                MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(mouseDelta.y);

                var roll = (Keyboard.current.qKey.isPressed ? 1.0f : 0.0f) + (Keyboard.current.eKey.isPressed ? -1.0f : 0.0f);
                MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(roll);
            }

            // 戦闘モードの切り替え
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
            {
                MessageBus.Instance.UserInputSetActorCombatMode.Broadcast(ActorCombatMode.Slip);
            }
            else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                MessageBus.Instance.UserInputSetActorCombatMode.Broadcast(ActorCombatMode.Fighter);
            }
            
            // マウスカーソルの切り替え
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Cockpit && !Keyboard.current.leftAltKey.isPressed)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }

        public void SetObservePlayerQuestData(PlayerQuestData playerQuestData)
        {
            observePlayerQuestData = playerQuestData;
            
            interactionList.SetObservePlayerQuestData(playerQuestData);
            inventoryView.SetObservePlayerQuestData(playerQuestData);
        }
        
        public void SetObserveAreaData(AreaData areaData)
        {
            mapPanelView.SetObserveAreaData(areaData);
            interactionList.SetObserveAreaData(areaData);
        }

        void OnClickMap()
        {
            MessageBus.Instance.UserInputSwitchMap.Broadcast();
        }
        
        void OnClickInteract()
        {
            MessageBus.Instance.UserInputSwitchInteractList.Broadcast();
        }
        
        void OnClickInventory()
        {
            MessageBus.Instance.UserInputSwitchInventory.Broadcast();
        }
        
        void OnClickSwitchActorModeCombatButton()
        {
            // Switchじゃなくてちゃんと指定したほうがいいかも
            MessageBus.Instance.UserInputSwitchActorMode.Broadcast();
        }
    }
}
