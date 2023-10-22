using System;

namespace VariableInventorySystem
{
    public static class GridLayoutHelper
    {
        public static (int WidthCount, int HeightCount) GetRotateDataSize(ICellData cellData)
        {
            switch (cellData)
            {
                case IGridCellData gridCellData:
                    return cellData.IsRotate
                        ? (gridCellData.WidthCount, gridCellData.HeightCount)
                        : (gridCellData.HeightCount, gridCellData.WidthCount);
                default:
                    throw new ArgumentException("Add calculate code for width and height from other ICellData types");
            }
        }
    }
}
