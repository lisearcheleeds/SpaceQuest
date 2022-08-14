using System;
using VariableInventorySystem;

namespace RoboQuest.Quest
{
    public class InventoryData
    {
        public Guid InstanceId { get; }
        public VariableInventoryViewData VariableInventoryViewData { get; }

        public InventoryData(int capacityWidth, int capacityHeight)
        {
            InstanceId = Guid.NewGuid();
            VariableInventoryViewData = new VariableInventoryViewData(capacityWidth, capacityHeight);
        }
    }
}