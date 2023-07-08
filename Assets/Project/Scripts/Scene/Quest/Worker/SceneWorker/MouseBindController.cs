using UnityEngine;
using UnityEngine.InputSystem;

namespace AloneSpace
{
    public class MouseBindController
    {
        UserData userData;

        public void Initialize(UserData userData)
        {
            this.userData = userData;

        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
            switch (userData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    CheckObserve();
                    CheckLookAtDistance();
                    break;
                case ActorOperationMode.Cockpit:
                    CheckCockpit();
                    CheckWeapon();
                    break;
                case ActorOperationMode.CockpitFreeCamera:
                    CheckCockpitFreeCamera();
                    CheckWeapon();
                    break;
                case ActorOperationMode.Spotter:
                    CheckSpotter();
                    CheckWeapon();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    CheckSpotterFreeCamera();
                    CheckWeapon();
                    break;
            }
        }

        void CheckObserve()
        {
            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);
        }

        void CheckCockpit()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();

            // 旋回操作
            // マウスだとroll操作がしたいだけなのにpitchがすごい反応してしまうみたいなのがあるのでちょっと補正を掛ける
            var pitch = userData.ControlActorData.ActorStateData.PitchBoosterPowerRatio;
            var pitchRate = Mathf.Clamp01(mouseDelta.x == 0 ? 1.0f : Mathf.Abs(mouseDelta.y / mouseDelta.x));
            var pitchInput = mouseDelta.y * pitchRate * 0.1f;
            pitch = Mathf.Clamp(pitch * 0.95f + pitchInput, -1.0f, 1.0f);

            var roll = userData.ControlActorData.ActorStateData.RollBoosterPowerRatio;
            var rollRate = Mathf.Clamp01(mouseDelta.y == 0 ? 1.0f : Mathf.Abs(mouseDelta.x / mouseDelta.y));
            var rollInput = mouseDelta.x * rollRate * 0.1f;
            roll = Mathf.Clamp(roll * 0.95f - rollInput, -1.0f, 1.0f);

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(pitch);
            // MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(roll);

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(userData.ControlActorData.Rotation * Vector3.forward);

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
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

        void CheckSpotter()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            if (localLookAtAngle.x < -90.0f)
            {
                localLookAtAngle.x = 180.0f + localLookAtAngle.x;
                localLookAtAngle.y += 180;
            }

            if (localLookAtAngle.x > 90.0f)
            {
                localLookAtAngle.x = 180.0f - localLookAtAngle.x;
                localLookAtAngle.y -= 180;
            }

            // Yaw側の成分が多かったらpitch成分を少なくする
            var lookAtDirection = userData.LookAtSpace * Quaternion.Euler(localLookAtAngle) * Vector3.forward;
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

            if (0.95f < forwardDot)
            {
                // おおよその方向が合致していたら上方向を合わせるRollに切り替えてピッチとヨーだけで調整する
                var rollRight = Vector3.Dot(userData.LookAtSpace * Vector3.up, userData.ControlActorData.Rotation * Vector3.right);
                rollValue = rollRight * -4.0f;
                yawValue = rightDot * 4.0f;
            }

            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(Quaternion.Lerp(userData.LookAtSpace, userData.ControlActorData.Rotation, 0.001f));
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(Mathf.Clamp(pitchValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(Mathf.Clamp(yawValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(Mathf.Clamp(rollValue, -1.0f, 1.0f));

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
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

        void Old()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            // localLookAtAngle.x = localLookAtAngle.x + mouseDelta.y * -1.0f;
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            // Yaw側の成分が多かったらpitch成分を少なくする
            var lookAtDirection = userData.LookAtSpace * Quaternion.Euler(localLookAtAngle) * Vector3.forward;
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

            if (0.95f < forwardDot)
            {
                // おおよその方向が合致していたら上方向を合わせるRollに切り替えてピッチとヨーだけで調整する
                var rollRight = Vector3.Dot(userData.LookAtSpace * Vector3.up, userData.ControlActorData.Rotation * Vector3.right);
                rollValue = rollRight * -4.0f;
                yawValue = rightDot * 4.0f;
            }

            // MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(Quaternion.Lerp(userData.LookAtSpace, userData.ControlActorData.Rotation, 0.001f));
            MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.LookAtSpace);
            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(Mathf.Clamp(pitchValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(Mathf.Clamp(yawValue, -1.0f, 1.0f));
            MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(Mathf.Clamp(rollValue, -1.0f, 1.0f));

            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }

        void CheckLookAtDistance()
        {
            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f);
        }

        void CheckWeapon()
        {
            if (Mouse.current.leftButton.isPressed && Cursor.lockState == CursorLockMode.Locked)
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(true);
            }
            else
            {
                MessageBus.Instance.UserInputSetExecuteWeapon.Broadcast(false);
            }
        }
    }
}
