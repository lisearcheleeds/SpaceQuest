﻿using System;
using UnityEngine;

namespace AloneSpace
{
    public interface IPositionData
    {
        Guid InstanceId { get; }
        
        int? AreaId { get; }
        Vector3 Position { get; }
        Quaternion Rotation { get; }

        void SetAreaId(int? areaId);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}
