using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class SceneInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Confined;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.None;

        protected override KeyBindKey[] UseBindKeys =>
            new[]
            {
                KeyBindKey.Menu, KeyBindKey.Menu, KeyBindKey.MenuStatusView, KeyBindKey.MenuInventoryView,
                KeyBindKey.MenuPlayerView, KeyBindKey.MenuAreaView, KeyBindKey.MenuMapView,
                KeyBindKey.ActorOperationModeSwitchObserve, KeyBindKey.ActorOperationModeSwitchCockpit,
                KeyBindKey.ActorOperationModeSwitchCockpitFreeCamera, KeyBindKey.ActorOperationModeSwitchSpotter,
                KeyBindKey.ActorOperationModeSwitchSpotterFreeCamera,
            };

        public override bool UpdatePointer()
        {
            return true;
        }

        public override bool UpdateKey(Key[] usedKey)
        {
            CheckMenu(usedKey);
            CheckActorOperationMode(usedKey);
            return true;
        }

        void CheckMenu(Key[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.Menu, usedKey))
            {
                // Openのみ
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
            }

            if (WasReleasedThisFrame(KeyBindKey.Menu, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInputCloseMenu.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuStatusView, usedKey))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuStatusView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuInventoryView, usedKey))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuInventoryView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuPlayerView, usedKey))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuPlayerView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuAreaView, usedKey))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuAreaView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuMapView, usedKey))
            {
                MessageBus.Instance.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInputSwitchMenuMapView.Broadcast();
            }
        }

        void CheckActorOperationMode(Key[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.Escape, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchObserve, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpit, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpitFreeCamera, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.CockpitFreeCamera);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotter, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotterFreeCamera, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotterFreeCamera);
            }
        }
    }
}
