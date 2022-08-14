using System;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class ItemInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;
        
        public Guid InstanceId { get; }

        public int AreaIndex { get; }
        public Vector3 Position { get; set; }
        public string Text => ItemData.ItemVO.Text;
        public float InteractTime => 3.0f;
        
        public ItemData ItemData { get; }

        public ItemInteractData(ItemData itemData, int areaId, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            ItemData = itemData;
            AreaIndex = areaId;
            Position = position;
        }
        
        public Vector3 GetClosestPoint(Vector3 position)
        {
            return Position;
        }

        public bool IsInteractionRange(Vector3 position)
        {
            return (position - Position).magnitude < InteractionRange;
        }
    }
}