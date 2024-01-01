﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace AloneSpace
{
    public class ActorOperationObserveFreeCameraInputLayer : ActorOperationInputLayer
    {
        public override CursorLockMode CursorLockMode => CursorLockMode.Confined;

        UserData userData;

        public ActorOperationObserveFreeCameraInputLayer(UserData userData)
        {
            this.userData = userData;
        }

        public override bool UpdatePointer()
        {
            CheckObserve();
            CheckLookAtDistance();

            return true;
        }

        public override bool UpdateKey(ButtonControl[] usedKey)
        {
            CheckObserveMoving(usedKey);
            return false;
        }

        void CheckObserve()
        {
            var mouseDelta = Mouse.current.delta.ReadValue();
            var localLookAtAngle = userData.LookAtAngle;

            localLookAtAngle.x = Mathf.Clamp(localLookAtAngle.x + mouseDelta.y * -1.0f, -90.0f, 90.0f);
            localLookAtAngle.y = Mathf.Repeat(localLookAtAngle.y + mouseDelta.x + 180, 360) - 180;
            localLookAtAngle.z = 0;

            MessageBus.Instance.UserCommandSetLookAtAngle.Broadcast(localLookAtAngle);
        }

        protected void CheckLookAtDistance()
        {
            MessageBus.Instance.UserCommandSetLookAtDistance.Broadcast(userData.LookAtDistance + Mouse.current.scroll.ReadValue().y * 0.1f);
        }
    }
}