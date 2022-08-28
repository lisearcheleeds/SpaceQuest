using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public interface ITarget
    {
        ITargetData TargetData { get; }
        Vector3 MoveDelta { get; }
    }
}
