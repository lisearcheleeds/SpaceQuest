using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VariableInventorySystem
{
    public abstract class GridView<TGridCellData> : MonoBehaviour, IView where TGridCellData : ICellData
    {
        [SerializeField] GridCell<TGridCellData> cellPrefab;

        [SerializeField] Vector2 gridCellSize = new Vector2(72, 72);
        [SerializeField] RectTransform resizeTarget;
        [SerializeField] RectTransform cellParent;

        public InventoryData InventoryData { get; private set; }

        protected List<GridCell<TGridCellData>> gridCells = new List<GridCell<TGridCellData>>();
        protected CellCornerType CellCornerType;

        public virtual void Initialize()
        {
        }

        public ICell CreateEffectCell()
        {
            return Instantiate(cellPrefab).GetComponent<GridCell<TGridCellData>>();
        }

        public virtual void Apply(InventoryData inventoryData)
        {
            InventoryData = inventoryData;

            resizeTarget.sizeDelta = new Vector2(InventoryData.CapacityWidth * gridCellSize.x, InventoryData.CapacityHeight * gridCellSize.y);

            gridCells = InventoryData.CellData.Select(cellData =>
            {
                var gridCell = Instantiate(cellPrefab, cellParent).GetComponent<GridCell<TGridCellData>>();
                gridCell.SetLocalPosition();
                gridCell.Apply(cellData);
                return gridCell;
            }).ToList();
        }

        public virtual void OnPrePick(ICellData cellData)
        {
        }

        public virtual bool OnPick(ICellData cellData)
        {
            if (cellData == null)
            {
                return false;
            }

            var id = InventoryData.GetId(cellData);
            return id.HasValue;
        }

        public virtual void OnDrag(ICell effectCell, PointerEventData cursorPosition)
        {
            var stareId = GetStareId(cursorPosition);
            if (!stareId.HasValue)
            {
                return;
            }

            CellCornerType = GetCorner(cursorPosition);
        }

        public virtual bool OnDrop(int? dropTargetId, ICellData cellData)
        {
            // check target;
            if (!dropTargetId.HasValue)
            {
                return false;
            }

            if (!InventoryData.CheckInsert(dropTargetId.Value, cellData))
            {
                return false;
            }

            // place
            InventoryData.InsertInventoryItem(dropTargetId.Value, cellData);
            var gridCell = Instantiate(cellPrefab, cellParent).GetComponent<GridCell<TGridCellData>>();
            gridCell.Apply(cellData);
            gridCell.SetLocalPosition();
            gridCells.Add(gridCell);
            return true;
        }

        public virtual void OnDropped(bool isDropped)
        {
            if (!isDropped && originalId.HasValue)
            {
                // revert
                itemViews[originalId.Value].Apply(originalCellData);
                InventoryData.InsertInventoryItem(originalId.Value, originalCellData);
            }

            originalId = null;
            originalCellData = null;
        }

        public virtual void OnCellEnter(ICell stareCell, ICell effectCell)
        {
        }

        public virtual void OnCellExit(ICell stareCell)
        {
            CellCornerType = CellCornerType.None;
        }

        public virtual int? GetStareId(PointerEventData cursorPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cellParent,
                cursorPosition.position,
                cursorPosition.enterEventCamera,
                out var localPosition);

            var positionX = (int)Mathf.Floor(localPosition.x / gridCellSize.x);
            var positionY = (int)Mathf.Floor(localPosition.y / gridCellSize.y);
            if (0 < positionX && positionX <= InventoryData.CapacityWidth && 0 < positionY && positionY <= InventoryData.CapacityHeight)
            {
                return positionX + positionY * InventoryData.CapacityWidth;
            }

            return null;
        }

        protected virtual int? GetIndex(ICell stareCell, ICellData effectCellData, CellCornerType cellCornerType)
        {
            var index = GetIndex(stareCell);

            // offset index
            var (widthCount, heightCount) = GridLayoutHelper.GetRotateDataSize(effectCellData);
            if (widthCount % 2 == 0)
            {
                if ((cellCornerType & CellCornerType.Left) != CellCornerType.None)
                {
                    index--;
                }
            }

            if (heightCount % 2 == 0)
            {
                if ((cellCornerType & CellCornerType.Top) != CellCornerType.None)
                {
                    index -= InventoryData.CapacityWidth;
                }
            }

            index -= (widthCount - 1) / 2;
            index -= (heightCount - 1) / 2 * InventoryData.CapacityWidth;
            return index;
        }

        protected virtual CellCornerType GetCorner(PointerEventData cursorPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                cellParent,
                cursorPosition.position,
                cursorPosition.enterEventCamera,
                out var viewLocalPosition);

            var cellLocalPosition = new Vector2(viewLocalPosition.x % gridCellSize.x, viewLocalPosition.y % gridCellSize.y);

            // depends on pivot
            var corner = CellCornerType.None;
            if (cellLocalPosition.x < Mathf.Epsilon)
            {
                corner |= CellCornerType.Left;
            }

            if (cellLocalPosition.x > Mathf.Epsilon)
            {
                corner |= CellCornerType.Right;
            }

            if (cellLocalPosition.y > Mathf.Epsilon)
            {
                corner |= CellCornerType.Top;
            }

            if (cellLocalPosition.y < Mathf.Epsilon)
            {
                corner |= CellCornerType.Bottom;
            }

            return corner;
        }

        protected virtual Vector3 GetEvenNumberOffset(int width, int height, float widthOffset, float heightOffset)
        {
            var evenNumberOffset = Vector3.zero;

            if (width % 2 == 0)
            {
                if ((CellCornerType & CellCornerType.Left) != CellCornerType.None)
                {
                    evenNumberOffset.x -= widthOffset;
                }

                if ((CellCornerType & CellCornerType.Right) != CellCornerType.None)
                {
                    evenNumberOffset.x += widthOffset;
                }
            }

            if (height % 2 == 0)
            {
                if ((CellCornerType & CellCornerType.Top) != CellCornerType.None)
                {
                    evenNumberOffset.y += heightOffset;
                }

                if ((CellCornerType & CellCornerType.Bottom) != CellCornerType.None)
                {
                    evenNumberOffset.y -= heightOffset;
                }
            }

            return evenNumberOffset;
        }
    }
}
