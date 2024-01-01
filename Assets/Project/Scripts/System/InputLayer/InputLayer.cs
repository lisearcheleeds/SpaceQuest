using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public abstract class InputLayer
    {
        public abstract CursorLockMode CursorLockMode { get; }
        public abstract InputLayerDuplicateGroup DuplicateGroup { get; }

        public abstract bool UpdatePointer();
        public abstract bool UpdateKey(ButtonControl[] usedKey);

        protected abstract KeyBindKey[] UseBindKeys { get; }

        public ButtonControl[] GetUsedKeys()
        {
            return UseBindKeys.Select(keyBindKey =>
            {
                if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
                {
                    Debug.LogWarning($"Key bind not found :{keyBindKey}");
                }

                return key;
            }).ToArray();
        }

        protected bool IsPressed(KeyBindKey keyBindKey, ButtonControl[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return key.isPressed;
        }

        protected bool WasPressedThisFrame(KeyBindKey keyBindKey, ButtonControl[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return key.wasPressedThisFrame;
        }

        protected bool WasReleasedThisFrame(KeyBindKey keyBindKey, ButtonControl[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return key.wasReleasedThisFrame;
        }
    }
}
