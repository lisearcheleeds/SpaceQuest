using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationFighterModeInputLayer : ActorOperationInputLayer
    {
        UserData userData;

        public ActorOperationFighterModeInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdateInput(ButtonControl[] usedKey)
        {
            if (IsPressed(KeyBindKey.FreeCamera, usedKey))
            {
                CheckCockpitFreeCamera();
            }
            else
            {
                CheckCockpit(usedKey);
            }

            CheckCockpitMoving(usedKey);
            CheckWeaponKeys(usedKey);

            return false;
        }

        void CheckCockpit(ButtonControl[] usedKey)
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
            MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.Broadcast((IsPressed(KeyBindKey.FighterModeYawPlus, usedKey) ? 1.0f : 0.0f) + (IsPressed(KeyBindKey.FighterModeYawMinus, usedKey) ? -1.0f : 0.0f));
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.Broadcast(roll);

            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.Broadcast(userData.ControlActorData.Rotation * Vector3.forward);

            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler.BoundingSize);
        }
        
        void CheckCockpitFreeCamera()
        {
            if (userData.ControlActorData == null)
            {
                return;
            }

            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserInput.UserInputPitchBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInput.UserInputYawBoosterPowerRatio.Broadcast(0);
            MessageBus.Instance.UserInput.UserInputRollBoosterPowerRatio.Broadcast(0);

            MessageBus.Instance.UserInput.UserCommandSetLookAtSpace.Broadcast(userData.ControlActorData.Rotation);
            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);

            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(userData.ControlActorData.ActorGameObjectHandler?.BoundingSize ?? 0);
        }
        
        void CheckCockpitMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInput.UserInputForwardBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.FighterModeForward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInput.UserInputBackBoosterPowerRatio.Broadcast(IsPressed(KeyBindKey.FighterModeBackward, usedKey) ? 1.0f : 0.0f);
            MessageBus.Instance.UserInput.UserInputRightBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputLeftBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputTopBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputBottomBoosterPowerRatio.Broadcast(0.0f);
        }
    }
}
