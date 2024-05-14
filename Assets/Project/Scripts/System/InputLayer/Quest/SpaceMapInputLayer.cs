using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class SpaceMapInputLayer : InputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.None;
        public override InputLayerDuplicateGroup DuplicateGroup => InputLayerDuplicateGroup.None;

        UserData userData;
        
        protected override KeyBindKey[] UseBindKeys => new[]
        {
            KeyBindKey.SpaceMapView, KeyBindKey.Escape,
        };

        public SpaceMapInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckCamera();
            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckMenu(usedKey);
            
            // 下位レイヤーの入力は全てブロック
            return true;
        }
        
        void CheckCamera()
        {
            if (Mouse.current.rightButton.isPressed)
            {
                var mouseDelta = Mouse.current.delta.ReadValue();
                var localLookAtAngle = userData.SpaceMapLookAtAngle;

                localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
                localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
                localLookAtAngle.z = 0;

                MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtAngle.Broadcast(localLookAtAngle);                
            }

            var scrollValue = Mouse.current.scroll.ReadValue();

            if (scrollValue.y != 0)
            {
                MessageBus.Instance.UserInput.UserCommandSetSpaceMapLookAtDistance.Broadcast(Mathf.Max(
                    0, 
                    userData.SpaceMapLookAtDistance + Mouse.current.scroll.ReadValue().y * -0.1f));
            }
        }

        void CheckMenu(ButtonControl[] usedKey)
        {
            if (WasPressedThisFrame(KeyBindKey.SpaceMapView, usedKey))
            {
                // Closeのみ
                MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.Broadcast();
            }

            if (WasPressedThisFrame(KeyBindKey.Escape, usedKey))
            {
                MessageBus.Instance.UserInput.UserInputCloseSpaceMapView.Broadcast();
            }
        }
    }
}
