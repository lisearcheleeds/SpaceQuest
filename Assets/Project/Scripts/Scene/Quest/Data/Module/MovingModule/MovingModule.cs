using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AloneSpace
{
    /// <summary>
    /// 毎フレーム呼び出される移動処理
    /// </summary>
    public class MovingModule : IModule
    {
        IPositionData positionData;
        Action<float> onBeginModuleUpdate;
        
        public Vector3 InertiaTensor { get; private set; }
        public Vector3 InertiaRotationAxis { get; private set; }
        public float InertiaRotationAngle { get; private set; }

        public Vector3 MoveDelta { get; private set; }

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

        public void SetInertiaTensor(Vector3 inertiaTensor)
        {
            InertiaTensor = inertiaTensor;
        }
        
        public void SetInertiaRotation(float inertiaRotationAngle, Vector3 axis)
        {
            InertiaRotationAngle = inertiaRotationAngle;
            InertiaRotationAxis = axis;
        }
        
        public void SetInertiaRotation(Quaternion inertiaRotation)
        {
            inertiaRotation.ToAngleAxis(out var angle, out var axis);
            SetInertiaRotation(angle, axis);
        }

        public void OnUpdateModule(float deltaTime)
        {
            onBeginModuleUpdate?.Invoke(deltaTime);
            
            MoveDelta = InertiaTensor * deltaTime;

            positionData.SetPosition(positionData.Position + MoveDelta);
            positionData.SetRotation(positionData.Rotation * Quaternion.AngleAxis(InertiaRotationAngle * deltaTime, InertiaRotationAxis));
        }
    }
}