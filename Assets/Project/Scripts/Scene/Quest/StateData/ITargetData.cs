using System;
using UnityEngine;

namespace AloneSpace
{
    public interface ITargetData : IPosition
    {
        Guid InstanceId { get; }
        bool IsAlive { get; }
        Vector3 MoveDelta { get; }
    }
}