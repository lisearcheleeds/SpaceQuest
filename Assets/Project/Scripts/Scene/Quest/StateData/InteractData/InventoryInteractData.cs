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

        public int? AreaId { get; }
        
        public Vector3 Position { get; set; }
        public string Text => $"Inventory (" + InventoryData.Sum(x => x.VariableInventoryViewData.CellData.Count(y => y != null)) + ")";
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.NearPosition;
        
        public InventoryData[] InventoryData { get; }

        public InventoryInteractData(InventoryData[] inventoryData, int areaId, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            InventoryData = inventoryData;
            AreaId = areaId;
            Position = position;
        }
        
        public Vector3 GetClosestPoint(IPositionData positionData)
        {
            return Position;
        }

        public bool IsInteractionRange(IPositionData positionData)
        {
            return (positionData.Position - Position).magnitude < InteractionRange;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }
}