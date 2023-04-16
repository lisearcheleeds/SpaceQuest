using System;
using UnityEngine;

namespace AloneSpace
{
    public class MovingModule : IModule
    {
        IPositionData positionData;
        Action<float> onBeginModuleUpdate;
        
        public Vector3 InertiaTensor { get; set; }
        public Quaternion InertiaTensorRotation { get; set; } = Quaternion.identity;

        public Vector3 MoveDelta;

        public MovingModule(IPositionData positionData, Action<float> onBeginModuleUpdate)
        {
            this.positionData = positionData;
            this.onBeginModuleUpdate = onBeginModuleUpdate;
        }

        public void ActivateModule()
        {
            MessageBus.Instance.RegisterMovingModule.Broadcast(this);
        }

        public void DeactivateModule()
        {
            MessageBus.Instance.UnRegisterMovingModule.Broadcast(this);
        }

        public void OnUpdateModule(float deltaTime)
        {
            onBeginModuleUpdate?.Invoke(deltaTime);
            
            positionData.SetPosition(positionData.Position + InertiaTensor);
            positionData.SetRotation(positionData.Rotation * InertiaTensorRotation);
        }
    }
}