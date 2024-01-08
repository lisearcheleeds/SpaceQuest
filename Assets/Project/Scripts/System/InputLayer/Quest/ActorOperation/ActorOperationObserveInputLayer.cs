using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationObserveInputLayer : ActorOperationInputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Confined;

        UserData userData;

        public ActorOperationObserveInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckLookAtDistance();

            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckObserveMoving(usedKey);
            return false;
        }

        protected void CheckLookAtDistance()
        {
            MessageBus.Instance.UserInput.UserCommandSetLookAtDistance.Broadcast(Mathf.Min(
                userData.ControlActorData.ActorGameObjectHandler?.BoundingSize ?? 0, 
                userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f));
        }
    }
}
