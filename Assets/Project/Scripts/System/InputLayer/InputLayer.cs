using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public abstract class InputLayer
    {
        public abstract CursorLockMode CursorLockMode { get; }
        public abstract InputLayerDuplicateGroup DuplicateGroup { get; }

        public abstract bool UpdatePointer();
        public abstract bool UpdateKey(Key[] usedKey);

        protected abstract KeyBindKey[] UseBindKeys { get; }

        public Key[] GetUsedKeys()
        {
            return UseBindKeys.Select(keyBindKey =>
            {
                if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
                {
                    Debug.LogWarning($"Key bind not found :{keyBindKey}");
                    key = Key.None;
                }

                return key;
            }).ToArray();
        }

        protected bool IsPressed(KeyBindKey keyBindKey, Key[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return Keyboard.current[key].isPressed;
        }

        protected bool WasPressedThisFrame(KeyBindKey keyBindKey, Key[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return Keyboard.current[key].wasPressedThisFrame;
        }

        protected bool WasReleasedThisFrame(KeyBindKey keyBindKey, Key[] usedKey)
        {
            if (!InputLayerController.Instance.KeyBindMap.TryGetValue(keyBindKey, out var key))
            {
                return false;
            }

            if (usedKey.Contains(key))
            {
                return false;
            }

            return Keyboard.current[key].wasReleasedThisFrame;
        }
    }
}
