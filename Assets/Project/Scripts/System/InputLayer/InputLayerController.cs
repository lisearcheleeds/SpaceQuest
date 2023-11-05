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

        List<InputLayer> reversedInputLayerStack = new List<InputLayer>();
        Key[][] keyUseKeyList;

        protected override void OnInitialize()
        {
            SetupKeyMap();
        }

        public void PushLayer(InputLayer inputLayer)
        {
            var currentDuplicateGroup = reversedInputLayerStack.FirstOrDefault()?.DuplicateGroup;
            if (currentDuplicateGroup != InputLayerDuplicateGroup.None && currentDuplicateGroup == inputLayer.DuplicateGroup)
            {
                PopLayer();
            }

            reversedInputLayerStack.Insert(0, inputLayer);

            Cursor.lockState = inputLayer.CursorLockMode;

            UpdateKeyUseKeyList();
        }

        public void PopLayer()
        {
            reversedInputLayerStack.Remove(reversedInputLayerStack.First());
            Cursor.lockState = reversedInputLayerStack.FirstOrDefault()?.CursorLockMode ?? CursorLockMode.None;

            UpdateKeyUseKeyList();
        }

        public void PopLayer(InputLayer target)
        {
            while (reversedInputLayerStack.Contains(target))
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
        }

        void UpdateKeyUseKeyList()
        {
            keyUseKeyList = new Key[reversedInputLayerStack.Count][];

            // 前レイヤーのキーをブロックする
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                keyUseKeyList[i] = i == 0
                    ? Array.Empty<Key>()
                    : keyUseKeyList[i - 1].Concat(reversedInputLayerStack[i - 1].GetUsedKeys()).ToArray();
            }
        }

        void Update()
        {
            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                if (reversedInputLayerStack[i].UpdatePointer())
                {
                    break;
                }
            }

            for (var i = 0; i < reversedInputLayerStack.Count; i++)
            {
                if (reversedInputLayerStack[i].UpdateKey(keyUseKeyList[i]))
                {
                    break;
                }
            }
        }
    }
}
