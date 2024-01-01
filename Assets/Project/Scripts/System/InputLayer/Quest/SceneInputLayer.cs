using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class SceneInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Confined;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.None;

        UserData userData;

        protected override KeyBindKey[] UseBindKeys =>
            new[]
            {
                KeyBindKey.Menu, KeyBindKey.Menu, KeyBindKey.MenuStatusView, KeyBindKey.MenuInventoryView,
                KeyBindKey.MenuPlayerView, KeyBindKey.MenuAreaView, KeyBindKey.MenuMapView,
                KeyBindKey.ActorOperationModeSwitchObserve, KeyBindKey.ActorOperationModeSwitchCockpit,
                KeyBindKey.ActorOperationModeSwitchFreeCamera, KeyBindKey.ActorOperationModeSwitchSpotter,
            };

        public SceneInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckMenu(usedKey);
            CheckActorOperationMode(usedKey);
            return true;
        }

        void CheckMenu(ButtonControl[] usedKey)
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

        void CheckActorOperationMode(ButtonControl[] usedKey)
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
            
            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotter, usedKey))
            {
                MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchFreeCamera, usedKey))
            {
                if (userData.ActorOperationMode == ActorOperationMode.Cockpit)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.CockpitFreeCamera);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.Spotter)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotterFreeCamera);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.Observe)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.ObserveFreeCamera);
                }
            }
            
            if (WasReleasedThisFrame(KeyBindKey.ActorOperationModeSwitchFreeCamera, usedKey))
            {
                if (userData.ActorOperationMode == ActorOperationMode.CockpitFreeCamera)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.SpotterFreeCamera)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.ObserveFreeCamera)
                {
                    MessageBus.Instance.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
                }
            }
        }
    }
}
