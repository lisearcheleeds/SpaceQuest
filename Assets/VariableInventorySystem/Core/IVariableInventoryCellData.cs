using System;

namespace VariableInventorySystem
{
    public interface IVariableInventoryCellData
    {
        int Id { get; }
        Guid InstanceId { get; }
        int Width { get; }
        int Height { get; }
        bool IsRotate { get; set; }
        IVariableInventoryAsset ImageAsset { get; }
    }
}
