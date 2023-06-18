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

        public void OnLateUpdate()
        {
            switch (userData.ActorOperationMode)
            {
                case ActorOperationMode.Observe:
                    CheckLookAtDistance();
                    break;
                case ActorOperationMode.Cockpit:
                    CheckCockpit();
                    CheckWeapon();
                    break;
                case ActorOperationMode.Spotter:
                    CheckSpotter();
                    CheckWeapon();
                    CheckLookAtDistance();
                    break;
                case ActorOperationMode.SpotterFreeCamera:
                    CheckSpotter();
                    CheckWeapon();
                    CheckLookAtDistance();
                    break;
            }
        }

        void CheckCockpit()
        {
        }

        void CheckSpotter()
        {
            // WASDとマウス
            // 右クリック前：カメラは自由
            // 右クリック中：カメラは自由、機体の回転が追従
            // 右クリック後：機体の回転にカメラ空間がリセット
            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = Quaternion.AngleAxis(-mouseDelta.y, Vector3.right)
                                   * Quaternion.AngleAxis(mouseDelta.x, Vector3.up)
                                   * userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = localLookAtAngle.y + mouseDelta.x;
            localLookAtAngle.z = 0;

            // ActorとUserDataそれぞれに同じ値を設定
            if (Mouse.current.rightButton.wasReleasedThisFrame && userData.ControlActorData != null)
            {
                // 角度基準リセット
                MessageBus.Instance.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
                MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(Vector3.zero);
            }
            else
            {
                MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);
            }

            if (Mouse.current.rightButton.isPressed && userData.ControlActorData != null)
            {
                // 追従
                var lookAtDirection = userData.LookAtSpace * Quaternion.Euler(userData.LookAtAngle) * Vector3.forward;
                MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(Vector3.Dot(lookAtDirection, userData.ControlActorData.Rotation * Vector3.up) < 0 ? 1.0f : -1.0f);
                MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(Vector3.Dot(lookAtDirection, userData.ControlActorData.Rotation * Vector3.left) < 0 ? 1.0f : -1.0f);
                MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(Vector3.Dot(userData.LookAtSpace * Vector3.up, userData.ControlActorData.Rotation * Vector3.right) < 0 ? 1.0f : -1.0f);
            }
            else
            {
                MessageBus.Instance.UserInputPitchBoosterPowerRatio.Broadcast(0);
                MessageBus.Instance.UserInputYawBoosterPowerRatio.Broadcast(0);

                var roll = (Keyboard.current.qKey.isPressed ? 1.0f : 0.0f) + (Keyboard.current.eKey.isPressed ? -1.0f : 0.0f);
                MessageBus.Instance.UserInputRollBoosterPowerRatio.Broadcast(roll);
            }
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
