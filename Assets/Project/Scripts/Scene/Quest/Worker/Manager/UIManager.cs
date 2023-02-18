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
            var forceDirection = Vector3.zero;
            if (Keyboard.current.wKey.isPressed) forceDirection += Vector3.forward;
            if (Keyboard.current.sKey.isPressed) forceDirection += Vector3.back;
            if (Keyboard.current.aKey.isPressed) forceDirection += Vector3.left;
            if (Keyboard.current.dKey.isPressed) forceDirection += Vector3.right;
            if (Keyboard.current.spaceKey.isPressed) forceDirection += Vector3.up;
            if (Keyboard.current.leftCtrlKey.isPressed) forceDirection += Vector3.down;
            MessageBus.Instance.UserInputDirectionAndRotation.Broadcast(forceDirection, Mouse.current.delta.ReadValue());

            // 戦闘モードの切り替え
            if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
            {
                MessageBus.Instance.UserInputSetActorCombatMode.Broadcast(ActorCombatMode.Standard);
            }
            else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
            {
                MessageBus.Instance.UserInputSetActorCombatMode.Broadcast(ActorCombatMode.Fighter);
            }
            
            // マウスカーソルの切り替え
            if (observePlayerQuestData.MainActorData.ActorMode == ActorMode.Combat && !Keyboard.current.leftAltKey.isPressed)
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
