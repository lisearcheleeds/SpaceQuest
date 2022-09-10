using UnityEngine;

namespace RoboQuest
{
    public interface IPosition
    {
        int AreaIndex { get; }
        Vector3 Position { get; }
    }
}