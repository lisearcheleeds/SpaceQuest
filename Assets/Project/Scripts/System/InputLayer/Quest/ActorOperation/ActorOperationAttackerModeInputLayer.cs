using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationAttackerModeInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationAttackerModeInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdateInput(ButtonControl[] usedKey)
        {
            CheckSpotter(usedKey);
            CheckSpotterMoving(usedKey);
            CheckWeaponKeys(usedKey);

            return false;
        }

        void CheckSpotterMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInput.UserInputForwardBoosterPowerRatio.Broadcast(1.0f);
            MessageBus.Instance.UserInput.UserInputBackBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputRightBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputLeftBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputTopBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputBottomBoosterPowerRatio.Broadcast(0.0f);
        }

        void CheckSpotter(ButtonControl[] usedKey)
        {
            var mouseDelta = Mouse.current.delta.ReadValue();

            // 視点
            var localLookAtAngle = userData.LookAtAngle;
            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            var verticalInput = (IsPressed(KeyBindKey.AttackerModeVerticalPlus, usedKey) ? 1.0f : 0.0f) +
                                (IsPressed(KeyBindKey.AttackerModeVerticalMinus, usedKey) ? -1.0f : 0.0f);
            var horizontalInput = (IsPressed(KeyBindKey.AttackerModeHorizontalPlus, usedKey) ? 1.0f : 0.0f) +
                                  (IsPressed(KeyBindKey.AttackerModeHorizontalMinus, usedKey) ? -1.0f : 0.0f);
            
            var actorUp = userData.ControlActorData.Rotation * Vector3.up;
            var actorForward = userData.ControlActorData.Rotation * Vector3.forward;
            var actorRight = userData.ControlActorData.Rotation * Vector3.right;
            
            var pitchOutput = 0.0f;
            var rollOutput = 0.0f;
            var yawOutput = 0.0f;
            
            // 垂直
            if (verticalInput != 0)
            {
                var targetDirection = verticalInput > 0 ? Vector3.up : Vector3.down;
                var rotationAxis = Vector3.Cross(actorForward, targetDirection).normalized;

                // ピッチとヨーで真上か真下を向く
                pitchOutput = Mathf.Sign(Vector3.Dot(rotationAxis, actorRight));
                yawOutput = Mathf.Sign(Vector3.Dot(rotationAxis, actorUp));
            }

            // 水平
            if (horizontalInput != 0)
            {
                // 左右入力がある場合はピッチ全開が前提
                pitchOutput = -1.0f;
                
                // 入力方向に90度ロール回転
                var upRotationAxis = Vector3.Cross(actorRight, Vector3.up).normalized;
                rollOutput += Mathf.Sign(Vector3.Dot(upRotationAxis, actorForward)) * horizontalInput;

                // 上下の傾きはヨーでなんとかする
                var targetForward = Vector3.ProjectOnPlane(actorForward, Vector3.up).normalized;
                var forwardRotationAxis = Vector3.Cross(actorForward, targetForward).normalized;
                yawOutput += Mathf.Sign(Vector3.Dot(forwardRotationAxis, actorUp));
            }
            
            MessageBus.Instance.UserInput.UserInputPitchBoosterPowerRatio.Broadcast(pitchOutput);
            MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.Broadcast(yawOutput);
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.Broadcast(rollOutput);
            
            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.Broadcast(Quaternion.identity);
            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            if (userData.ControlActorData != null)
            {
                MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
            }
        }
    }
}
