using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class AreaInteractData : IInteractData
    {
        public Guid InstanceId { get; }

        public int? AreaId => AreaData.AreaId;

        public Vector3 Position { get; }
        public string Text { get; }
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;
        
        public AreaData AreaData { get; }

        public AreaInteractData(AreaData areaData, AreaData fromAreaData)
        {
            InstanceId = Guid.NewGuid();
            AreaData = areaData;

            if (fromAreaData != null)
            {
                Position = (fromAreaData.StarSystemPosition - areaData.StarSystemPosition).normalized * 100.0f;
            }
            else
            {
                Position = new Vector3(
                    Random.Range(-areaData.SpaceSize.x, areaData.SpaceSize.x), 
                    Random.Range(-areaData.SpaceSize.y, areaData.SpaceSize.y),
                    Random.Range(-areaData.SpaceSize.z, areaData.SpaceSize.z));
            }

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