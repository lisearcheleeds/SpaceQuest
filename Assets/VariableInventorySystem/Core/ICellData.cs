namespace VariableInventorySystem
{
    public interface ICellData
    {
        bool IsRotate { get; set; }

        int GridCellDataSizeWidth { get; }
        int GridCellDataSizeHeight { get; }
    }
}
