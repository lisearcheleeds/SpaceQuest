using UnityEngine;

namespace AloneSpace
{
    public interface IPosition
    {
        int AreaId { get; }
        Vector3 Position { get; }
    }
}
