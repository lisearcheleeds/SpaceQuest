using UnityEngine;

namespace VariableInventorySystem
{
    public abstract class GridCell : Cell
    {
        public IGridCellData GridCellData => (IGridCellData)CellData;
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
                gridCellSize.x * (GridCellData?.WidthCount ?? 1),
                gridCellSize.y * (GridCellData?.HeightCount ?? 1));
        }

        protected override Vector2 GetCenterOffset()
        {
            var (rotatedWidthCount, rotatedHeightCount) = GridLayoutHelper.GetRotateDataSize(GridCellData);
            return new Vector2(
                -(rotatedWidthCount - 1) * GridCellData.WidthCount * 0.5f,
                (rotatedHeightCount - 1) * GridCellData.HeightCount * 0.5f);
        }
    }
}
