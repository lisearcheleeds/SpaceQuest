using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class InputLayerController : MonoSingleton<InputLayerController>
    {
        public Dictionary<KeyBindKey, Key> KeyBindMap = new Dictionary<KeyBindKey, Key>();

        List<InputLayerPare> reversedInputLayerStack = new List<InputLayerPare>();
        bool isDirty;

        class InputLayerPare
        {
            public InputLayer InputLayer { get; }
            public Key[] UsedKeys { get; set; }

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
            KeyBindMap.Clear();

            // FIXME: 設定で変えられるようにする
            KeyBindMap[KeyBindKey.Forward] = Key.W;
            KeyBindMap[KeyBindKey.Backward] = Key.S;
            KeyBindMap[KeyBindKey.Right] = Key.D;
            KeyBindMap[KeyBindKey.Left] = Key.A;
            KeyBindMap[KeyBindKey.Up] = Key.Space;
            KeyBindMap[KeyBindKey.Down] = Key.LeftCtrl;

            KeyBindMap[KeyBindKey.PitchPlus] = Key.UpArrow;
            KeyBindMap[KeyBindKey.PitchMinus] = Key.DownArrow;
            KeyBindMap[KeyBindKey.YawPlus] = Key.D;
            KeyBindMap[KeyBindKey.YawMinus] = Key.A;
            KeyBindMap[KeyBindKey.RollPlus] = Key.RightArrow;
            KeyBindMap[KeyBindKey.RollMinus] = Key.LeftArrow;

            KeyBindMap[KeyBindKey.Reload] = Key.R;
            KeyBindMap[KeyBindKey.WeaponGroup1] = Key.Digit1;
            KeyBindMap[KeyBindKey.WeaponGroup2] = Key.Digit2;
            KeyBindMap[KeyBindKey.WeaponGroup3] = Key.Digit3;
            KeyBindMap[KeyBindKey.WeaponGroup4] = Key.Digit4;
            KeyBindMap[KeyBindKey.WeaponGroup5] = Key.Digit5;

            KeyBindMap[KeyBindKey.Menu] = Key.Tab;
            KeyBindMap[KeyBindKey.MenuStatusView] = Key.F1;
            KeyBindMap[KeyBindKey.MenuInventoryView] = Key.F2;
            KeyBindMap[KeyBindKey.MenuPlayerView] = Key.F3;
            KeyBindMap[KeyBindKey.MenuAreaView] = Key.F4;
            KeyBindMap[KeyBindKey.MenuMapView] = Key.F5;

            KeyBindMap[KeyBindKey.MouseModeSwitch] = Key.LeftAlt;

            KeyBindMap[KeyBindKey.ActorOperationModeSwitchObserve] = Key.Z;
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchCockpit] = Key.X;
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchCockpitFreeCamera] = Key.C;
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchSpotter] = Key.V;
            KeyBindMap[KeyBindKey.ActorOperationModeSwitchSpotterFreeCamera] = Key.B;

            KeyBindMap[KeyBindKey.Escape] = Key.Escape;
        }

        void UpdateKeyUseKeyList()
        {
            // 前レイヤーのキーをブロックする
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                reversedInputLayerStack[i].UsedKeys = i == 0
                    ? Array.Empty<Key>()
                    : reversedInputLayerStack[i - 1].UsedKeys.Concat(reversedInputLayerStack[i - 1].InputLayer.GetUsedKeys()).ToArray();
            }

            isDirty = true;
        }

        void Update()
        {
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                if (reversedInputLayerStack[i].InputLayer.UpdatePointer() || isDirty)
                {
                    break;
                }
            }

            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                if (reversedInputLayerStack[i].InputLayer.UpdateKey(reversedInputLayerStack[i].UsedKeys) || isDirty)
                {
                    break;
                }
            }

            isDirty = false;
        }
    }
}
