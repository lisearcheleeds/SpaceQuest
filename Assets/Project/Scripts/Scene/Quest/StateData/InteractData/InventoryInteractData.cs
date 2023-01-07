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

        public int AreaId { get; }
        
        public Vector3 Position { get; set; }
        public string Text => $"Inventory (" + InventoryData.Sum(x => x.VariableInventoryViewData.CellData.Count(y => y != null)) + ")";
        public float InteractTime => 3.0f;
        public InteractRestraintType InteractRestraintType => InteractRestraintType.CantMove;
        
        public InventoryData[] InventoryData { get; }

        public InventoryInteractData(InventoryData[] inventoryData, IPosition position)
        {
            InstanceId = Guid.NewGuid();

            InventoryData = inventoryData;
            AreaId = position.AreaId;
            Position = position.Position;
        }
        
        public Vector3 GetClosestPoint(IPosition position)
        {
            return Position;
        }

        public bool IsInteractionRange(IPosition position)
        {
            return (position.Position - Position).magnitude < InteractionRange;
        }

        public void SetPosition(Vector3 position)
        {
            Position = position;
        }
    }
}