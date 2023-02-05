using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractData : IPositionData
    {
        Guid InstanceId { get; }
        string Text { get; }
        float InteractTime { get; }

        InteractRestraintType InteractRestraintType { get; }

        Vector3 GetClosestPoint(IPositionData positionData);
        bool IsInteractionRange(IPositionData positionData);
        void SetPosition(Vector3 position);
        void SetRotation(Quaternion rotation);
    }
}