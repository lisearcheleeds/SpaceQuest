using System;
using VariableInventorySystem;

namespace AloneSpace.Common
{
    public class DropAreaCellData : IVariableInventoryCellData
    {
        public int Id => 0;
        public Guid InstanceId { get; }
        public bool IsRotate { get; set; }
        public int Width => 0;
        public int Height => 0;
        IVariableInventoryAsset IVariableInventoryCellData.ImageAsset => null;

        public DropAreaCellData()
        {
            InstanceId = Guid.NewGuid();
        }
    }
}
