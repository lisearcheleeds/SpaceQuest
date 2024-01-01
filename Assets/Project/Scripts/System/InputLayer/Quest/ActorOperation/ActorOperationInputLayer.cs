using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public abstract class ActorOperationInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Locked;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.ActorOperation;

        protected override KeyBindKey[] UseBindKeys =>
            new[]
            {
                KeyBindKey.Forward, KeyBindKey.Backward, KeyBindKey.Right, KeyBindKey.Left, KeyBindKey.Up,
                KeyBindKey.Down, KeyBindKey.YawPlus, KeyBindKey.YawMinus, KeyBindKey.Forward, KeyBindKey.Backward,
                KeyBindKey.Right, KeyBindKey.Left, KeyBindKey.Up, KeyBindKey.Down, KeyBindKey.Trigger,
                KeyBindKey.Trigger, KeyBindKey.Reload, KeyBindKey.WeaponGroup1, KeyBindKey.WeaponGroup2,
                KeyBindKey.WeaponGroup3, KeyBindKey.WeaponGroup4, KeyBindKey.WeaponGroup5
            };

        protected void CheckObserveMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(0.0f);
        }

        protected void CheckCockpitMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Forward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Backward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(0.0f);
            
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast((IsPressed(KeyBindKey.YawPlus, usedKey) ? 1.0f : 0.0f) + (IsPressed(KeyBindKey.YawMinus, usedKey) ? -1.0f : 0.0f));
        }

        protected void CheckSpotterMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInputForwardBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Forward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBackBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Backward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputRightBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Right, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputLeftBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Left, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputTopBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Up, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInputBottomBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.Down, usedKey) ? 1.0f : 0.0f);
        }

        protected virtual void CheckWeaponKeys(ButtonControl[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.Trigger, usedKey))
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(true);
            }
            else if (WasReleasedThisFrame(KeyBindKey.Trigger, usedKey))
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);
            }

            if (WasPressedThisFrame(KeyBindKey.Reload, usedKey))
            {
                MessageBus.Instance.UserInputReloadWeapon.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup1, usedKey))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(0);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup2, usedKey))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(1);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup3, usedKey))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(2);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup4, usedKey))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(3);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup5, usedKey))
            {
                MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.Broadcast(4);
            }
        }

        protected virtual void CheckWeapon()
        {
            if (Mouse.current.leftButton.isPressed && Cursor.lockState == CursorLockMode.Locked)
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(true);
            }
            else
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);
            }
        }
    }
}
