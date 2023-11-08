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
                        ? (WidthCount: gridCellData.GridCellDataSizeWidth, HeightCount: gridCellData.GridCellDataSizeHeight)
                        : (HeightCount: gridCellData.GridCellDataSizeHeight, WidthCount: gridCellData.GridCellDataSizeWidth);
                default:
                    throw new ArgumentException("Add calculate code for width and height from other ICellData types");
            }
        }
    }
}
