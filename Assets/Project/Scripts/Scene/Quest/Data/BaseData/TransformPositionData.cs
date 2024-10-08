﻿using System;
using UnityEngine;

namespace AloneSpace
{
    public class TransformPositionData : IPositionData
    {
        public Guid InstanceId { get; }
        public int? AreaId => areaId.HasValue ? areaId.Value : original.AreaId;
        public Vector3 Position => transform != null ? transform.position : default;
        public Quaternion Rotation => transform != null ? transform.rotation : default;

        int? areaId;
        IPositionData original;
        Transform transform;

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
