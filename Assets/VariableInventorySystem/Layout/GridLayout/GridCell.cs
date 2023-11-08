using UnityEngine;

namespace VariableInventorySystem
{
    public abstract class GridCell<TGridCellData> : Cell where TGridCellData : ICellData
    {
        public TGridCellData GridCellData => (TGridCellData)CellData;
        public override Vector2 CellSize => cellDataSize;
        protected override RectTransform SizeRoot => sizeRoot;
        protected override RectTransform RotateRoot => content;

        [SerializeField] Vector2 gridCellSize;
        [SerializeField] RectTransform sizeRoot;
        [SerializeField] RectTransform content;

        Vector2 cellDataSize;

        protected override void OnApply()
        {
            content.gameObject.SetActive(GridCellData != null);

            cellDataSize = new Vector2(
                gridCellSize.x * (GridCellData?.GridCellDataSizeWidth ?? 1),
                gridCellSize.y * (GridCellData?.GridCellDataSizeHeight ?? 1));
        }

        protected override Vector2 GetCenterOffset()
        {
            var (rotatedWidthCount, rotatedHeightCount) = GridLayoutHelper.GetRotateDataSize(GridCellData);
            return new Vector2(
                -(rotatedWidthCount - 1) * GridCellData.GridCellDataSizeWidth * 0.5f,
                (rotatedHeightCount - 1) * GridCellData.GridCellDataSizeHeight * 0.5f);
        }
    }
}
