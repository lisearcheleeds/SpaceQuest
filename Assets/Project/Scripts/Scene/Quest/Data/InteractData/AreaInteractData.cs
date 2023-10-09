using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class AreaInteractData : IInteractData
    {
        public Guid InstanceId { get; }

        public MovingModule MovingModule => null;

        public int? AreaId => AreaData.AreaId;
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public string Text { get; }
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;

        public AreaData AreaData { get; }

        public AreaInteractData(AreaData areaData, Vector3 currentStarSystemPosition)
        {
            InstanceId = Guid.NewGuid();
            AreaData = areaData;

            Position = currentStarSystemPosition - areaData.StarSystemPosition;
            Rotation = Quaternion.identity;

            Text = $"Area: {AreaId}";
        }

        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            throw new NotSupportedException();
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return true;
        }

        public void SetAreaId(int? areaId)
        {
            throw new NotSupportedException();
        }

        public void SetPosition(Vector3 position)
        {
            throw new NotSupportedException();
        }

        public void SetRotation(Quaternion rotation)
        {
            throw new NotSupportedException();
        }

        public void ActivateModules()
        {
        }

        public void DeactivateModules()
        {
        }
    }
}
