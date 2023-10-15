using System;
using UnityEngine;

namespace VariableInventorySystem
{
    public abstract class VariableInventoryCell : MonoBehaviour, IVariableInventoryCell
    {
        public RectTransform RectTransform => (RectTransform)transform;
        public IVariableInventoryCellData CellData { get; private set; }

        public abstract Vector2 CellSize { get; }

        public Vector2 CellDataSize
        {
            get
            {
                if (CellData == null)
                {
                    return new Vector2(CellSize.x, CellSize.y);
                }

                return new Vector2(CellData.Width * CellSize.x, CellData.Height * CellSize.y);
            }
        }

        public Vector2 RotateCellDataSize
        {
            get
            {
                var isRotate = CellData?.IsRotate ?? false;
                var cellSize = CellDataSize;
                if (isRotate)
                {
                    var tmp = cellSize.x;
                    cellSize.x = cellSize.y;
                    cellSize.y = tmp;
                }

                return cellSize;
            }
        }

        protected virtual IVariableInventoryCellActions ButtonActions { get; set; }

        public void SetCellCallback(
            Action<IVariableInventoryCell> onPointerClick,
            Action<IVariableInventoryCell> onPointerOptionClick,
            Action<IVariableInventoryCell> onPointerEnter,
            Action<IVariableInventoryCell> onPointerExit)
        {
            ButtonActions.SetCallback(
                () => onPointerClick?.Invoke(this),
                () => onPointerOptionClick?.Invoke(this),
                () => onPointerEnter?.Invoke(this),
                () => onPointerExit?.Invoke(this));
        }

        public void Apply(IVariableInventoryCellData cellData)
        {
            CellData = cellData;
            OnApply();
        }

        public abstract void SetSelectable(bool value);

        protected abstract void OnApply();
    }
}
