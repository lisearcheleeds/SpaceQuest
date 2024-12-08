using UnityEngine;
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
                KeyBindKey.ActorOperationModeSwitchObserverMode, KeyBindKey.ActorOperationModeSwitchFighterMode,
                KeyBindKey.ActorOperationModeSwitchAttackerMode, KeyBindKey.FreeCamera
            };

        public SceneInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdateInput(ButtonControl[] usedKey)
        {
            CheckMenu(usedKey);
            CheckActorOperationMode(usedKey);
            CheckActorControl(usedKey);
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
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.ObserverMode);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchObserverMode, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.ObserverMode);
            }

            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchFighterMode, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.FighterMode);
            }
            
            if (WasPressedThisFrame(KeyBindKey.ActorOperationModeSwitchAttackerMode, usedKey))
            {
                MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.AttackerMode);
            }
        }

        void CheckActorControl(ButtonControl[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.LockOn, usedKey))
            {
                if (userData.ActorOperationMode == ActorOperationMode.AttackerMode)
                {
                    MessageBus.Instance.UserInput.UserCommandSetActorOperationMode.Broadcast(ActorOperationMode.LockOnMode);
                }
            }
        }
    }
}
