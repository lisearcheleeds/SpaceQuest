using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationSpotterFreeCameraInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationSpotterFreeCameraInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckSpotterFreeCamera();
            CheckWeapon();

            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckSpotterMoving(usedKey);
            CheckWeaponKeys(usedKey);
            return false;
        }

        void CheckSpotterFreeCamera()
        {
            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var localLookAtAngle = userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(0);

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(Quaternion.identity);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }
    }
}
