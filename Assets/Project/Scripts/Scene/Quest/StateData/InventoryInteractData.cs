﻿using System;
using System.Linq;
using UnityEngine;

namespace RoboQuest.Quest
{
    public class InventoryInteractData : IInteractData
    {
        static readonly float InteractionRange = 2.0f;
        
        public Guid InstanceId { get; }

        public int AreaIndex { get; }
        public Vector3 Position { get; set; }
        public string Text => $"Inventory (" + InventoryData.Sum(x => x.VariableInventoryViewData.CellData.Count(y => y != null)) + ")";
        public float InteractTime => 3.0f;
        
        public InventoryData[] InventoryData { get; }

        public InventoryInteractData(InventoryData[] inventoryData, int areaId, Vector3 position)
        {
            InstanceId = Guid.NewGuid();

            InventoryData = inventoryData;
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