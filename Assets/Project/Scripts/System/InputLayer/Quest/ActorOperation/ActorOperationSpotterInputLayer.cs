using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationSpotterInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationSpotterInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckSpotter();
            CheckWeapon();

            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckSpotterMoving(usedKey);
            CheckWeaponKeys(usedKey);
            return false;
        }

        void CheckSpotter()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var lookAtSpace = Quaternion.identity;
            var localLookAtAngle = userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            // Yaw側の成分が多かったらpitch成分を少なくする
            var lookAtDirection = lookAtSpace * Quaternion.Euler(localLookAtAngle) * Vector3.forward;
            var upDot = Vector3.Dot(lookAtDirection, userData.ControlActorData.Rotation * Vector3.up);
            var rightDot = Vector3.Dot(lookAtDirection, userData.ControlActorData.Rotation * Vector3.right);
            var forwardDot = Vector3.Dot(lookAtDirection, userData.ControlActorData.Rotation * Vector3.forward);

            var upDotAbs = Mathf.Abs(upDot);
            var rightDotAbs = Mathf.Abs(rightDot);

            // ピッチ量
            var enablePitch =
                (0.1f < upDotAbs && rightDotAbs < 0.1f) ||
                (upDotAbs < 0.1f && rightDotAbs < 0.01f);
            var pitchValue = enablePitch ? upDot * -4.0f : 0;

            // ロール量
            var rollValue = -0.8f < upDot ? rightDot * -4.0f : rightDot * 4.0f;

            // ヨー量
            var yawValue = 0.0f;

            if (0.9f < forwardDot)
            {
                // おおよその方向が合致していたら上方向を合わせるRollに切り替えてロールとヨーだけで調整する
                var rollRight = Vector3.Dot(lookAtSpace * Vector3.up, userData.ControlActorData.Rotation * Vector3.right);
                rollValue = rollRight * -4.0f;
                yawValue = rightDot * 4.0f;
            }

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(Mathf.Clamp(pitchValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(Mathf.Clamp(yawValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(Mathf.Clamp(rollValue, -1.0f, 1.0f));

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(lookAtSpace);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData?.ActorGameObjectHandler.BoundingSize ?? 0);
        }
    }
}
