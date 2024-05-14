using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class MenuInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.None;

        protected override KeyBindKey[] UseBindKeys => new[]
        {
            KeyBindKey.Menu, KeyBindKey.MenuStatusView, KeyBindKey.MenuInventoryView, KeyBindKey.MenuPlayerView,
            KeyBindKey.Backward, KeyBindKey.Escape,
        };

        Func<UI.MenuView.MenuElement> getCurrentMenuElement;

        public MenuInputLayer(Func<UI.MenuView.MenuElement> getCurrentMenuElement)
        {
            this.getCurrentMenuElement = getCurrentMenuElement;
        }

        public override bool UpdatePointer()
        {
            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckMenu(usedKey);

            // 下位レイヤーの入力は全てブロック
            return true;
        }

        void CheckMenu(ButtonControl[] usedKey)
        {
            if (WasReleasedThisFrame(KeyBindKey.Menu, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuStatusView, usedKey))
            {
                if (getCurrentMenuElement() == UI.MenuView.MenuElement.StatusView)
                {
                    MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInput.UserInputSwitchMenuStatusView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuInventoryView, usedKey))
            {
                if (getCurrentMenuElement() == UI.MenuView.MenuElement.InventoryView)
                {
                    MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInput.UserInputSwitchMenuInventoryView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuPlayerView, usedKey))
            {
                if (getCurrentMenuElement() == UI.MenuView.MenuElement.PlayerView)
                {
                    MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInput.UserInputSwitchMenuPlayerView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.Escape, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
            }
        }
    }
}
