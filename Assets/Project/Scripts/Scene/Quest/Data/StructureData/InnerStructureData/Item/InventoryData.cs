using System;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryData
    {
        public Guid InstanceId { get; }
        public VariableInventorySystem.InventoryData Inventory { get; }

        public InventoryData(int capacityWidth, int capacityHeight)
        {
            InstanceId = Guid.NewGuid();
            Inventory = new VariableInventorySystem.InventoryData(capacityWidth, capacityHeight);
        }
    }
}
