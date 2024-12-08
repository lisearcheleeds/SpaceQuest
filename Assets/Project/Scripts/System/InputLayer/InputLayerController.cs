using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class InputLayerController : MonoSingleton<InputLayerController>
    {
        public Dictionary<KeyBindKey, ButtonControl> KeyBindMap = new Dictionary<KeyBindKey, ButtonControl>();

        List<InputLayerPare> reversedInputLayerStack = new List<InputLayerPare>();
        bool isDirty;

        class InputLayerPare
        {
            public InputLayer InputLayer { get; }
            public ButtonControl[] UsedKeys { get; set; }

            public InputLayerPare(InputLayer inputLayer)
            {
                InputLayer = inputLayer;
            }
        }

        protected override void OnInitialize()
        {
            SetupKeyMap();
        }

        public void PushLayer(InputLayer inputLayer)
        {
            var currentDuplicateGroup = reversedInputLayerStack.FirstOrDefault()?.InputLayer.DuplicateGroup;
            if (currentDuplicateGroup != InputLayerDuplicateGroup.None && currentDuplicateGroup == inputLayer.DuplicateGroup)
            {
                PopLayer();
            }

            reversedInputLayerStack.Insert(0, new InputLayerPare(inputLayer));

            Cursor.lockState = inputLayer.CursorLockMode;

            UpdateKeyUseKeyList();
            Debug.Log("PushLayer:" + string.Join(" -> ", reversedInputLayerStack.Select(x => x.InputLayer.GetType())));
        }

        public void PopLayer()
        {
            reversedInputLayerStack.Remove(reversedInputLayerStack.First());
            Cursor.lockState = reversedInputLayerStack.FirstOrDefault()?.InputLayer.CursorLockMode ?? CursorLockMode.None;

            UpdateKeyUseKeyList();
            Debug.Log("PopLayer:" + string.Join(" -> ", reversedInputLayerStack.Select(x => x.InputLayer.GetType())));
        }

        public void PopLayer(InputLayer target)
        {
            while (reversedInputLayerStack.Any(x => x.InputLayer == target))
            {
                PopLayer();
            }
        }

        public void SetupKeyMap()
        {
            // FIXME: 設定で変えられるようにする
            KeyBindMap.Clear();
            
            KeyBindMap[KeyBindKey.FighterModeForward] = Keyboard.current[Key.W];
            KeyBindMap[KeyBindKey.FighterModeBackward] = Keyboard.current[Key.S];

            KeyBindMap[KeyBindKey.AttackerModeVerticalPlus] = Keyboard.current[Key.W];
            KeyBindMap[KeyBindKey.AttackerModeVerticalMinus] = Keyboard.current[Key.S];
            KeyBindMap[KeyBindKey.AttackerModeHorizontalPlus] = Keyboard.current[Key.A];
            KeyBindMap[KeyBindKey.AttackerModeHorizontalMinus] = Keyboard.current[Key.D];
            
            KeyBindMap[KeyBindKey.AimModeForward] = Keyboard.current[Key.W];
            KeyBindMap[KeyBindKey.AimModeBackward] = Keyboard.current[Key.S];
            KeyBindMap[KeyBindKey.AimModeRight] = Keyboard.current[Key.D];
            KeyBindMap[KeyBindKey.AimModeLeft] = Keyboard.current[Key.A];
            KeyBindMap[KeyBindKey.AimModeUp] = Keyboard.current[Key.Space];
            KeyBindMap[KeyBindKey.AimModeDown] = Keyboard.current[Key.LeftCtrl];

            KeyBindMap[KeyBindKey.Trigger] = Mouse.current.leftButton;
            KeyBindMap[KeyBindKey.Reload] = Keyboard.current[Key.R];
            KeyBindMap[KeyBindKey.WeaponGroup1] = Keyboard.current[Key.Digit1];
            KeyBindMap[KeyBindKey.WeaponGroup2] = Keyboard.current[Key.Digit2];
            KeyBindMap[KeyBindKey.WeaponGroup3] = Keyboard.current[Key.Digit3];
            KeyBindMap[KeyBindKey.WeaponGroup4] = Keyboard.current[Key.Digit4];
            KeyBindMap[KeyBindKey.WeaponGroup5] = Keyboard.current[Key.Digit5];

            KeyBindMap[KeyBindKey.Menu] = Keyboard.current[Key.Tab];
            KeyBindMap[KeyBindKey.MenuStatusView] = Keyboard.current[Key.F1];
            KeyBindMap[KeyBindKey.MenuInventoryView] = Keyboard.current[Key.F2];
            KeyBindMap[KeyBindKey.MenuPlayerView] = Keyboard.current[Key.F3];
            KeyBindMap[KeyBindKey.SpaceMapView] = Keyboard.current[Key.M];

            KeyBindMap[KeyBindKey.MouseModeSwitch] = Keyboard.current[Key.LeftAlt];

            KeyBindMap[KeyBindKey.ActorOperationModeSwitchObserverMode] = Keyboard.current[Key.Z];
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchFighterMode] = Keyboard.current[Key.X];
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchAttackerMode] = Keyboard.current[Key.C];
            
            KeyBindMap[KeyBindKey.LockOn] = Mouse.current.rightButton;
            KeyBindMap[KeyBindKey.FreeCamera] = Mouse.current.middleButton;

            KeyBindMap[KeyBindKey.Escape] = Keyboard.current[Key.Escape];
        }

        void UpdateKeyUseKeyList()
        {
            // 前レイヤーのキーをブロックする
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                reversedInputLayerStack[i].UsedKeys = i == 0
                    ? Array.Empty<ButtonControl>()
                    : reversedInputLayerStack[i - 1].UsedKeys.Concat(reversedInputLayerStack[i - 1].InputLayer.GetUsedKeys()).ToArray();
            }

            isDirty = true;
        }

        void Update()
        {
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                if (reversedInputLayerStack[i].InputLayer.UpdateInput(reversedInputLayerStack[i].UsedKeys) || isDirty)
                {
                    break;
                }
            }

            isDirty = false;
        }
    }
}
