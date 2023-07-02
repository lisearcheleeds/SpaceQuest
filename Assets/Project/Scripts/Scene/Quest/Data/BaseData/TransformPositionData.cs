using System;
using UnityEngine;

namespace AloneSpace
{
    public class TransformPositionData : IPositionData
    {
        public Guid InstanceId { get; }

        public int? AreaId => areaId.HasValue ? areaId.Value : original.AreaId;

        public Vector3 Position
        {
            get
            {
                if (transform != null)
                {
                    position = transform.position;
                }

                return position;
            }
        }

        public Quaternion Rotation
        {
            get
            {
                if (transform != null)
                {
                    rotation = transform.rotation;
                }

                return rotation;
            }
        }

        int? areaId;
        IPositionData original;
        Transform transform;

        Vector3 position;
        Quaternion rotation;

        public TransformPositionData(int? areaId, Transform transform)
        {
            InstanceId = new Guid();

            this.areaId = areaId;
            this.original = null;
            this.transform = transform;
        }

        public TransformPositionData(IPositionData original, Transform transform)
        {
            InstanceId = new Guid();

            this.areaId = null;
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
