using UnityEngine;

namespace AloneSpace
{
    public interface IPosition
    {
        int AreaIndex { get; }
        Vector3 Position { get; }
    }
}