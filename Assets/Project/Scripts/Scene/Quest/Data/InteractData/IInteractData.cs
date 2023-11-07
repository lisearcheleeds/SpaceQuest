using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractData : IPositionData, IContentQuickViewData, IMovingModuleHolder
    {
        Guid InstanceId { get; }

        // Module
        MovingModule MovingModule { get; }

        InteractRestraintType InteractRestraintType { get; }

        string Text { get; }
        float InteractTime { get; }

        Vector3 GetClosestPoint(IPositionData positionData);
        bool IsInteractionRange(IPositionData positionData);

        void ActivateModules();
        void DeactivateModules();
    }
}
