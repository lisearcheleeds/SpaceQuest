namespace VariableInventorySystem
{
    public interface IGridCellData : ICellData
    {
        int WidthCount { get; }
        int HeightCount { get; }
    }
}
