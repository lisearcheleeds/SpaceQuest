using System;
using UnityEngine;

namespace AloneSpace
{
    public interface IInteractData
    {
        Guid InstanceId { get; }
        int AreaIndex { get; }
        Vector3 Position { get; set; }
        string Text { get; }
        float InteractTime { get; }
        
        Vector3 GetClosestPoint(Vector3 position);
        bool IsInteractionRange(Vector3 position);
    }
}