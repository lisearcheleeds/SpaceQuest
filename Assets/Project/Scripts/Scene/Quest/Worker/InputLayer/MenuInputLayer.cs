using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class MenuInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.None;

        protected override KeyBindKey[] UseBindKeys => new[]
        {
            KeyBindKey.Menu, KeyBindKey.MenuStatusView, KeyBindKey.MenuInventoryView, KeyBindKey.MenuPlayerView,
            KeyBindKey.MenuAreaView, KeyBindKey.MenuMapView,
        };

        Func<MenuView.MenuElement> getCurrentMenuElement;

        public MenuInputLayer(Func<MenuView.MenuElement> getCurrentMenuElement)
        {
            this.getCurrentMenuElement = getCurrentMenuElement;
        }

        public override bool UpdatePointer()
        {
            return true;
        }

        public override bool UpdateKey(Key[] usedKey)
        {
            CheckMenu(usedKey);

            // 下位レイヤーの入力は全てブロック
            return true;
        }

        void CheckMenu(Key[] usedKey)
        {
            if (WasReleasedThisFrame(KeyBindKey.Menu, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInputCloseMenu.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuStatusView, usedKey))
            {
                if (getCurrentMenuElement() == MenuView.MenuElement.StatusView)
                {
                    MessageBus.Instance.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInputSwitchMenuStatusView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuInventoryView, usedKey))
            {
                if (getCurrentMenuElement() == MenuView.MenuElement.InventoryView)
                {
                    MessageBus.Instance.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInputSwitchMenuInventoryView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuPlayerView, usedKey))
            {
                if (getCurrentMenuElement() == MenuView.MenuElement.PlayerView)
                {
                    MessageBus.Instance.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInputSwitchMenuPlayerView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuAreaView, usedKey))
            {
                if (getCurrentMenuElement() == MenuView.MenuElement.AreaView)
                {
                    MessageBus.Instance.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInputSwitchMenuAreaView.Broadcast();
                }
            }

            if (WasPressedThisFrame(KeyBindKey.MenuMapView, usedKey))
            {
                if (getCurrentMenuElement() == MenuView.MenuElement.MapView)
                {
                    MessageBus.Instance.UserInputCloseMenu.Broadcast();
                }
                else
                {
                    MessageBus.Instance.UserInputSwitchMenuMapView.Broadcast();
                }
            }
        }
    }
}
