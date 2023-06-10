using System;
using UnityEngine;

namespace AloneSpace
{
    public class TransformPositionData : IPositionData
    {
        IPositionData original;
        Transform transform;

        public Guid InstanceId { get; }

        public int? AreaId => original.AreaId;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;
        
        public TransformPositionData(IPositionData original, Transform transform)
        {
            InstanceId = new Guid();

            this.original = original;
            this.transform = transform;
        }

        public void SetAreaId(int? areaId)
        {
            throw new NotImplementedException();
        }

        public void SetPosition(Vector3 position)
        {
            throw new NotImplementedException();
        }

        public void SetRotation(Quaternion rotation)
        {
            throw new NotImplementedException();
        }
    }
}