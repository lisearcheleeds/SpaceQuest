﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

        public override bool UpdateKey(Key[] usedKey)
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
    }
}