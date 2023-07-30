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

            keyBindMap[KeyBindKey.PitchPlus] = Key.UpArrow;
            keyBindMap[KeyBindKey.PitchMinus] = Key.DownArrow;
            keyBindMap[KeyBindKey.YawPlus] = Key.D;
            keyBindMap[KeyBindKey.YawMinus] = Key.A;
            keyBindMap[KeyBindKey.RollPlus] = Key.RightArrow;
            keyBindMap[KeyBindKey.RollMinus] = Key.LeftArrow;

            keyBindMap[KeyBindKey.Reload] = Key.R;
            keyBindMap[KeyBindKey.WeaponGroup1] = Key.Digit1;
            keyBindMap[KeyBindKey.WeaponGroup2] = Key.Digit2;
            keyBindMap[KeyBindKey.WeaponGroup3] = Key.Digit3;
            keyBindMap[KeyBindKey.WeaponGroup4] = Key.Digit4;
            keyBindMap[KeyBindKey.WeaponGroup5] = Key.Digit5;

            keyBindMap[KeyBindKey.Menu] = Key.Tab;
            keyBindMap[KeyBindKey.MenuStatusView] = Key.F1;
            keyBindMap[KeyBindKey.MenuInventoryView] = Key.F2;
            keyBindMap[KeyBindKey.MenuPlayerView] = Key.F3;
            keyBindMap[KeyBindKey.MenuAreaView] = Key.F4;
            keyBindMap[KeyBindKey.MenuMapView] = Key.F5;

            keyBindMap[KeyBindKey.MouseModeSwitch] = Key.LeftAlt;

            keyBindMap[KeyBindKey.ActorOperationModeSwitchObserve] = Key.Z;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchCockpit] = Key.X;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchCockpitFreeCamera] = Key.C;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchSpotter] = Key.V;
            keyBindMap[KeyBindKey.ActorOperationModeSwitchSpotterFreeCamera] = Key.B;
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
            switch (userData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    CheckActorOperationMode();
                    CheckMenu();
                    break;
                case ActorOperationMode.Cockpit:
                    CheckCockpitMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    CheckMenu();
                    break;
                case ActorOperationMode.CockpitFreeCamera:
                    CheckCockpitMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    CheckMenu();
                    break;
                case ActorOperationMode.Spotter:
                    CheckSpotterMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    CheckMenu();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    CheckSpotterMoving();
                    CheckWeapon();
                    CheckMouseMode();
                    CheckActorOperationMode();
                    CheckMenu();
                    break;
            }
        }

        void CheckCockpitMoving()
        {
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Forward) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Backward) ? 1.0f : 0.0f);
            // MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Right) ? 1.0f : 0.0f);
            // MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Left) ? 1.0f : 0.0f);
            // MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Up) ? 1.0f : 0.0f);
            // MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Down) ? 1.0f : 0.0f);

            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast((IsPressed(KeyBindKey.YawPlus) ? 1.0f : 0.0f) + (IsPressed(KeyBindKey.YawMinus) ? -1.0f : 0.0f));
        }

        void CheckSpotterMoving()
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
            var isChanged = false;

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchObserve))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
                Cursor.lockState = CursorLockMode.None;
                isChanged = true;
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpit))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);
                Cursor.lockState = CursorLockMode.Locked;
                isChanged = true;
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpitFreeCamera))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.CockpitFreeCamera);
                Cursor.lockState = CursorLockMode.Locked;
                isChanged = true;
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotter))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
                Cursor.lockState = CursorLockMode.Locked;
                isChanged = true;
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotterFreeCamera))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotterFreeCamera);
                Cursor.lockState = CursorLockMode.Locked;
                isChanged = true;
            }

            if (isChanged)
            {
                // TODO: ここでやることじゃないのでやること決まったら移動
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);

                MessageBus.Instance.UserInputCloseMenu.Broadcast();
            }
        }

        void CheckMenu()
        {
            if (WasPressedThisFrame(KeyBindKey.Menu))
            {
                // Openのみ
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
            }

            if (WasReleasedThisFrame(KeyBindKey.Menu))
            {
                // Closeのみ
                MessageBus.Instance.UserInputCloseMenu.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuStatusView))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuStatusView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuInventoryView))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuInventoryView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuPlayerView))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuPlayerView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuAreaView))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuAreaView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuMapView))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuMapView.Broadcast();
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
