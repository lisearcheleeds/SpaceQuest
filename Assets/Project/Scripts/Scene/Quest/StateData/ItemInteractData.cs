using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class ItemInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;
        
        public Guid InstanceId { get; }

        public int AreaId { get; }
        public Vector3 Position { get; set; }
        public string Text => ItemData.ItemVO.Text;
        public float InteractTime => 3.0f;
        
        public ItemData ItemData { get; }

        public ItemInteractData(ItemData itemData, int areaId, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            ItemData = itemData;
            AreaId = areaId;
            Position = position;
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