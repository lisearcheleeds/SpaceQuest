using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class KeyBindController
    {
        UserData userData;

        // TODO: ちゃんとしたところに移動すｒ
        Dictionary<KeyBindKey, Key> keyBindMap = new Dictionary<KeyBindKey, Key>();

        public void Initialize(UserData userData)
        {
            this.userData = userData;

            keyBindMap[KeyBindKey.Forward] = Key.W;
            keyBindMap[KeyBindKey.Backward] = Key.S;
            keyBindMap[KeyBindKey.Right] = Key.D;
            keyBindMap[KeyBindKey.Left] = Key.A;
            keyBindMap[KeyBindKey.Up] = Key.Space;
            keyBindMap[KeyBindKey.Down] = Key.LeftCtrl;

            keyBindMap[KeyBindKey.Reload] = Key.R;
            keyBindMap[KeyBindKey.WeaponGroup1] = Key.Digit1;
            keyBindMap[KeyBindKey.WeaponGroup2] = Key.Digit2;
            keyBindMap[KeyBindKey.WeaponGroup3] = Key.Digit3;
            keyBindMap[KeyBindKey.WeaponGroup4] = Key.Digit4;
            keyBindMap[KeyBindKey.WeaponGroup5] = Key.Digit5;

            keyBindMap[KeyBindKey.Map] = Key.M;
            keyBindMap[KeyBindKey.InteractList] = Key.Tab;
            keyBindMap[KeyBindKey.Inventory] = Key.I;

            keyBindMap[KeyBindKey.MouseModeSwitch] = Key.LeftAlt;

            keyBindMap[KeyBindKey.ActorOperationModeSwitchObserve] = Key.Z;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchCockpit] = Key.X;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchSpotter] = Key.C;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchSpotFreeCamera] = Key.V;
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate()
        {
            switch (userData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    CheckActorOperationMode();
                    CheckUI();
                    break;
                case ActorOperationMode.Cockpit:
                    CheckMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    break;
                case ActorOperationMode.Spotter:
                    CheckMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    CheckMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    break;
            }
        }

        void CheckMoving()
        {
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Forward) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Backward) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Right) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Left) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Up) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Down) ? 1.0f : 0.0f);
        }

        void CheckWeapon()
        {
            if (WasPressedThisFrame(KeyBindKey.Trigger))
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(true);
            }
            else if (WasReleasedThisFrame(KeyBindKey.Trigger))
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);
            }

            if (WasPressedThisFrame(KeyBindKey.Reload))
            {
                MessageBus.Instance.UserInputReloadWeapon.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup1))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(0);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup2))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(1);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup3))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(2);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup4))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(3);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup5))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(4);
            }
        }

        void CheckMouseMode()
        {
            if (WasPressedThisFrame(KeyBindKey.MouseModeSwitch))
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.None;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }

        void CheckActorOperationMode()
        {
            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchObserve))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);

                // TODO: ここでやることじゃないのでやること決まったら移動
                Cursor.lockState = CursorLockMode.None;
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);

                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();

                MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Default);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpit))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);

                // TODO: ここでやることじゃないのでやること決まったら移動
                Cursor.lockState = CursorLockMode.Locked;
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);

                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();

                MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Cockpit);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotter))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);

                // TODO: ここでやることじゃないのでやること決まったら移動
                Cursor.lockState = CursorLockMode.Locked;
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);

                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();

                MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Cockpit);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotFreeCamera))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotterFreeCamera);

                // TODO: ここでやることじゃないのでやること決まったら移動
                Cursor.lockState = CursorLockMode.Locked;
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);

                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();

                MessageBus.Instance.UserCommandSetCameraMode.Broadcast(CameraMode.Cockpit);
            }
        }

        void CheckUI()
        {
            if (WasPressedThisFrame(KeyBindKey.Map))
            {
                MessageBus.Instance.UserInputSwitchMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.InteractList))
            {
                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputSwitchInventory.Broadcast();
                MessageBus.Instance.UserInputCloseInteractList.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.Inventory))
            {
                MessageBus.Instance.UserInputCloseMap.Broadcast();
                MessageBus.Instance.UserInputCloseInventory.Broadcast();
                MessageBus.Instance.UserInputSwitchInteractList.Broadcast();
            }
        }

        bool IsPressed(KeyBindKey keyBindKey)
        {
            return keyBindMap.ContainsKey(keyBindKey) && Keyboard.current[keyBindMap[keyBindKey]].isPressed;
        }

        bool WasPressedThisFrame(KeyBindKey keyBindKey)
        {
            return keyBindMap.ContainsKey(keyBindKey) && Keyboard.current[keyBindMap[keyBindKey]].wasPressedThisFrame;
        }

        bool WasReleasedThisFrame(KeyBindKey keyBindKey)
        {
            return keyBindMap.ContainsKey(keyBindKey) && Keyboard.current[keyBindMap[keyBindKey]].wasReleasedThisFrame;
        }
    }
}
