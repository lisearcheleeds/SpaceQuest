using System;
using UnityEngine;

namespace AloneSpace
{
    public class AreaInteractData : IInteractData
    {
        public Guid InstanceId { get; }

        public int? AreaId => AreaData.AreaId;
        
        public Vector3 Position => Vector3.zero;
        public string Text { get; }
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;
        
        public AreaData AreaData { get; }

        public AreaInteractData(AreaData areaData, AreaData fromAreaData)
        {
            InstanceId = Guid.NewGuid();
            AreaData = areaData;

            Text = $"Load to Area{areaData.AreaId} from {fromAreaData?.AreaId}";
        }
        
        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            throw new NotSupportedException();
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return true;
        }

        public void SetPosition(Vector3 position)
        {
            throw new NotSupportedException();
        }
    }
}