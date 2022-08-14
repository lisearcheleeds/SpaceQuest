using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    public interface ITarget
    {
        ITargetData TargetData { get; }
        Vector3 MoveDelta { get; }
    }
}
