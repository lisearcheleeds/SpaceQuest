using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
            CheckObserve();
            CheckLookAtDistance();

            return true;
        }

        public override bool UpdateKey(Key[] usedKey)
        {
            return false;
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

        protected void CheckLookAtDistance()
        {
            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f);
        }
    }
}
