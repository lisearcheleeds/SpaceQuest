using System;
using UnityEngine;

namespace RoboQuest
{
    public interface ITargetData
    {
        Guid InstanceId { get; }
        Vector3 Position { get; }
        bool IsTargetable { get; }
    }
}