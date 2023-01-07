using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractData : IPosition
    {
        Guid InstanceId { get; }
        string Text { get; }
        float InteractTime { get; }

        InteractRestraintType InteractRestraintType { get; }

        Vector3 GetClosestPoint(IPosition position);
        bool IsInteractionRange(IPosition position);
        void SetPosition(Vector3 position);
    }
}