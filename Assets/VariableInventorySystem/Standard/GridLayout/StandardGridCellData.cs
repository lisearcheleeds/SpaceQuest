namespace VariableInventorySystem
{
    public abstract class StandardGridCellData : IGridCellData
    {
        public abstract bool IsRotate { get; set; }
        public abstract int WidthCount { get; }
        public abstract int HeightCount { get; }
        public abstract string ImagePath { get; }
    }
}
