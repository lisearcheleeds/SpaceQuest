using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationCockpitInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationCockpitInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckCockpit();
            CheckWeapon();

            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckCockpitMoving(usedKey);
            CheckWeaponKeys(usedKey);
            return false;
        }

        void CheckCockpit()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();
            var mouseDeltaNormal = mouseDelta.normalized;

            // 旋回操作
            var pitch = userData.ControlActorData.ActorStateData.PitchBoosterPowerRatio;
            var pitchInput = mouseDelta.y * 0.1f * Mathf.Abs(mouseDeltaNormal.y);
            pitch = Mathf.Clamp(pitch + pitchInput, -1.0f, 1.0f);

            var roll = userData.ControlActorData.ActorStateData.RollBoosterPowerRatio;
            var rollInput = mouseDelta.x * 0.1f * Mathf.Abs(mouseDeltaNormal.x);
            roll = Mathf.Clamp(roll - rollInput, -1.0f, 1.0f);

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(pitch);
            // MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(roll);

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(userData.ControlActorData.Rotation * Vector3.forward);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }
    }
}
