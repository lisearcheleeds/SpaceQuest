﻿using UnityEngine;
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

        public void Initialize(QuestData questData, UserData userData)
        {
            this.userData = userData;

            cameraAngleControllerEffect.Initialize();

            weaponDataListView.Initialize();
            targetView.Initialize();

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

            // WASDとマウス
            if (userData.ActorOperationMode != ActorOperationMode.Observe)
            {
                MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(Keyboard.current.wKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(Keyboard.current.sKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(Keyboard.current.dKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(Keyboard.current.aKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(Keyboard.current.spaceKey.isPressed ? 1.0f : 0.0f);
                MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(Keyboard.current.leftCtrlKey.isPressed ? 1.0f : 0.0f);

                // 右クリック前：カメラは自由
                // 右クリック中：カメラは自由、機体の回転が追従
                // 右クリック後：機体の回転にカメラ空間がリセット
                var mouseDelta = Mouse.current.delta.ReadValue();
                var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                       * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                       * userData.LookAtAngle;

                localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
                localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
                localLookAtAngle.z = 0;

                // ActorとUserDataそれぞれに同じ値を設定
                if (Mouse.current.rightButton.wasReleasedThisFrame)
                {
                    // 角度基準リセット
                    MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.PlayerData.MainActorData.Rotation);
                    MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(Vector3.zero);
                }
                else
                {
                    MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);
                }

                MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f);

                MessageBus.Instance.ActorCommandSetLookAtDirection.Broadcast(
                    userData.PlayerData.MainActorData.InstanceId,
                    userData.LookAtSpace * Quaternion.Euler(localLookAtAngle) * Vector3.forward);

                if (Mouse.current.rightButton.isPressed)
                {
                    // 追従
                    var lookAtDirection = userData.LookAtSpace * Quaternion.Euler(userData.LookAtAngle) * Vector3.forward;
                    MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(Vector3.Dot(lookAtDirection, userData.PlayerData.MainActorData.Rotation * Vector3.up) < 0 ? 1.0f : -1.0f);
                    MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(Vector3.Dot(lookAtDirection, userData.PlayerData.MainActorData.Rotation * Vector3.left) < 0 ? 1.0f : -1.0f);
                    MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(Vector3.Dot(userData.LookAtSpace * Vector3.up, userData.PlayerData.MainActorData.Rotation * Vector3.right) < 0 ? 1.0f : -1.0f);
                }
                else
                {
                    MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(0);
                    MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);

                    var roll = (Keyboard.current.qKey.isPressed ? 1.0f : 0.0f) + (Keyboard.current.eKey.isPressed ? -1.0f : 0.0f);
                    MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(roll);
                }

                if (Keyboard.current.rKey.wasPressedThisFrame)
                {
                    MessageBus.Instance.UserInputReloadWeapon.Broadcast();
                }

                if (Mouse.current.leftButton.isPressed && !Keyboard.current.leftAltKey.isPressed)
                {
                    MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(true);
                }
                else
                {
                    MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);
                }

                if (Keyboard.current.digit1Key.wasPressedThisFrame)
                {
                    MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(0);
                }

                if (Keyboard.current.digit2Key.wasPressedThisFrame)
                {
                    MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(1);
                }

                if (Keyboard.current.digit3Key.wasPressedThisFrame)
                {
                    MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(2);
                }
            }

            if (userData.ActorOperationMode == ActorOperationMode.Spot || userData.ActorOperationMode == ActorOperationMode.SpotFreeCamera)
            {
                // 戦闘モードの切り替え
                if (Keyboard.current.leftShiftKey.wasPressedThisFrame)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spot);
                }
                else if (Keyboard.current.leftShiftKey.wasReleasedThisFrame)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotFreeCamera);
                }
            }

            // マウスカーソルの切り替え
            if (userData.ActorOperationMode != ActorOperationMode.Observe && !Keyboard.current.leftAltKey.isPressed)
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
            }
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
            switch (userData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotFreeCamera);
                    break;
                case ActorOperationMode.Cockpit:
                case ActorOperationMode.Spot:
                case ActorOperationMode.SpotFreeCamera:
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
                    break;
            }
        }
    }
}
