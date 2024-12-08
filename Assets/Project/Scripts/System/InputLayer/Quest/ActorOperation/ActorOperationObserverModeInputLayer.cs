using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationObserverModeInputLayer : ActorOperationInputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Confined;

        UserData userData;

        public ActorOperationObserverModeInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdateInput(ButtonControl[] usedKey)
        {
            CheckObserve();
            CheckLookAtDistance();
            CheckObserveMoving(usedKey);

            return false;
        }

        void CheckObserveMoving(ButtonControl[] usedKey)
        {
            MessageBus.Instance.UserInput.UserInputForwardBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputBackBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputRightBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputLeftBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputTopBoosterPowerRatio.Broadcast(0.0f);
            MessageBus.Instance.UserInput.UserInputBottomBoosterPowerRatio.Broadcast(0.0f);
        }

        void CheckObserve()
        {
            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserInput.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);
        }

        void CheckLookAtDistance()
        {
            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(Mathf.Min(
                userData.ControlActorData?.ActorGameObjectHandler?.BoundingSize ?? 0, 
                userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f));
        }
    }
}
