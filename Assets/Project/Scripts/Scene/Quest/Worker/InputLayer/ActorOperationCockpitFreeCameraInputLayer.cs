using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class ActorOperationCockpitFreeCameraInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationCockpitFreeCameraInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckCockpitFreeCamera();
            CheckWeapon();

            return true;
        }

        public override bool UpdateKey(Key[] usedKey)
        {
            CheckCockpitMoving(usedKey);
            CheckWeaponKeys(usedKey);
            return false;
        }

        void CheckCockpitFreeCamera()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(0);
            // MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(0);

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler?.BoundingSize ?? 0);
        }
    }
}
