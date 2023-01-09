using UnityEngine;

namespace AloneSpace
{
    public interface IPositionData
    {
        int? AreaId { get; }
        Vector3 Position { get; }
    }
}
