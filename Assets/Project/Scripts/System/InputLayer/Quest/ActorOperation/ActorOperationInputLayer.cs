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
                KeyBindKey.Trigger, KeyBindKey.Reload, KeyBindKey.WeaponGroup1, KeyBindKey.WeaponGroup2,
                KeyBindKey.WeaponGroup3, KeyBindKey.WeaponGroup4, KeyBindKey.WeaponGroup5
            };

        protected virtual void CheckWeaponKeys(ButtonControl[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.Trigger, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetExecuteWeapon.Broadcast(true);
            }
            else if (WasReleasedThisFrame(KeyBindKey.Trigger, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetExecuteWeapon.Broadcast(false);
            }

            if (WasPressedThisFrame(KeyBindKey.Reload, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputReloadWeapon.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup1, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.Broadcast(0);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup2, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.Broadcast(1);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup3, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.Broadcast(2);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup4, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.Broadcast(3);
            }

            if (WasPressedThisFrame(KeyBindKey.WeaponGroup5, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.Broadcast(4);
            }
        }
    }
}
