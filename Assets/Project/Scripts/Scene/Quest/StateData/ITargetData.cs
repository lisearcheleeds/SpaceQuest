using System;
using UnityEngine;

namespace AloneSpace
{
    public interface ITargetData : IPositionData
    {
        Guid InstanceId { get; }
        bool IsAlive { get; }
        Vector3 MoveDelta { get; }
    }
}