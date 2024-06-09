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

            // 旋回操作 操作感がいい感じになるのでAbsしたNormalを掛ける
            var pitchInput = mouseDelta.y * 0.1f * Mathf.Abs(mouseDeltaNormal.y);
            var pitch = Mathf.Clamp(userData.ControlActorData.ActorStateData.PitchBoosterPowerRatio + pitchInput, -1.0f, 1.0f);

            var rollInput = mouseDelta.x * 0.1f * Mathf.Abs(mouseDeltaNormal.x);
            var roll = Mathf.Clamp(userData.ControlActorData.ActorStateData.RollBoosterPowerRatio - rollInput, -1.0f, 1.0f);

            MessageBus.Instance.UserInput.UserInputPitchBoosterPowerRatio.Broadcast(pitch);
            // MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.Broadcast(roll);

            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.Broadcast(userData.ControlActorData.Rotation * Vector3.forward);

            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }
    }
}
