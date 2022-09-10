using System;
using UnityEngine;

namespace RoboQuest
{
    public interface ITargetData : IPosition
    {
        Guid InstanceId { get; }
    }
}