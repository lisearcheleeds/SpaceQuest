using System;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InventoryInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;
        
        public Guid InstanceId { get; }

        public int? AreaId { get; private set; }
        public Vector3 Position { get; private set; }
        public Quaternion Rotation { get; private set; }
        public string Text => $"Inventory (" + InventoryData.Sum(x => x.VariableInventoryViewData.CellData.Count(y => y != null)) + ")";
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;
        
        public InventoryData[] InventoryData { get; }

        public InventoryInteractData(InventoryData[] inventoryData, int areaId, Vector3 position, Quaternion rotation)
        {
            InstanceId = Guid.NewGuid();

            InventoryData = inventoryData;
            AreaId = areaId;
            Position = position;
            Rotation = rotation;
        }
        
        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            return Position;
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return (positionData.Position - Position).magnitude < InteractionRange;
        }

        public void SetAreaId(int? areaId)
        {
            AreaId = areaId;
        }
        
        public void SetPosition(Vector3 position)
        {
            Position = position;
        }

        public void SetRotation(Quaternion rotation)
        {
            Rotation = rotation;
        }
    }
}