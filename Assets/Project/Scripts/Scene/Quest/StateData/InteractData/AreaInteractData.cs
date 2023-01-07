using System;
using UnityEngine;

namespace AloneSpace
{
    public class AreaInteractData : IInteractData
    {
        public Guid InstanceId => throw new NotSupportedException();
        
        public int AreaId => AreaData.AreaId;
        
        public Vector3 Position => AreaData.Position;
        public string Text => AreaData.AreaId.ToString();
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.CantOtherAllAndCancel;
        
        public AreaData AreaData { get; }

        public AreaInteractData(AreaData areaData)
        {
            AreaData = areaData;
        }
        
        public Vector3 GetClosestPoint(IPosition position)
        {
            throw new NotSupportedException();
        }

        public bool IsInteractionRange(IPosition position)
        {
            return true;
        }

        public void SetPosition(Vector3 position)
        {
            throw new NotSupportedException();
        }
    }
}