namespace VariableInventorySystem
{
    /// <summary>
    /// If you want a VariableInventorySystem, you need IGridCellData, not ICellData.
    /// </summary>
    public interface ICellData
    {
        bool IsRotate { get; set; }
    }
}
