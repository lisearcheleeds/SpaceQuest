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
                KeyBindKey.MenuPlayerView, KeyBindKey.SpaceMapView,
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
                MessageBus.Instance.UserInput.UserInputOpenMenu.Broadcast();
            }

            if (WasReleasedThisFrame(KeyBindKey.Menu, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInput.UserInputCloseMenu.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuStatusView, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInput.UserInputSwitchMenuStatusView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuInventoryView, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInput.UserInputSwitchMenuInventoryView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.MenuPlayerView, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputOpenMenu.Broadcast();
                MessageBus.Instance.UserInput.UserInputSwitchMenuPlayerView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.SpaceMapView, usedKey))
            {
                // Openのみ
                MessageBus.Instance.UserInput.UserInputOpenSpaceMapView.Broadcast();
            }

            if (WasReleasedThisFrame(KeyBindKey.SpaceMapView, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.Broadcast();
            }
        }

        void CheckActorOperationMode(ButtonControl[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.Escape, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchObserve, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchCockpit, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);
            }
            
            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchSpotter, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchFreeCamera, usedKey))
            {
                if (userData.ActorOperationMode == ActorOperationMode.Cockpit)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.CockpitFreeCamera);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.Spotter)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.SpotterFreeCamera);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.Observe)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.ObserveFreeCamera);
                }
            }
            
            if (WasReleasedThisFrame(KeyBindKey.ActorOperationModeSwitchFreeCamera, usedKey))
            {
                if (userData.ActorOperationMode == ActorOperationMode.CockpitFreeCamera)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Cockpit);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.SpotterFreeCamera)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Spotter);
                }
                else if (userData.ActorOperationMode == ActorOperationMode.ObserveFreeCamera)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.Observe);
                }
            }
        }
    }
}
