using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public override bool UpdateKey(Key[] usedKey)
        {
            CheckSpotterMoving(usedKey);
            CheckWeaponKeys(usedKey);
            return false;
        }

        void CheckSpotterFreeCamera()
        {
            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.LookAtSpace);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(0);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }
    }
}
