using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class GridView : MonoBehaviour, IView
    {
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] RectTransform resizeTarget;

        [SerializeField] GridCell cellPrefab;

        public InventoryData InventoryData { get; private set; }

        public int CellCount => InventoryData.CapacityWidth * InventoryData.CapacityHeight;

        protected GridCell[] itemViews;
        protected CellCornerType CellCornerType;

        int? originalId;
        ICellData originalCellData;

        ICellEventListener listener;

        public virtual void Initialize()
        {
        }

        public ICell CreateEffectCell()
        {
            return Instantiate(cellPrefab).GetComponent<GridCell>();
        }

        public void SetCellEventListener(ICellEventListener listener)
        {
            this.listener = listener;
        }

        public virtual void Apply(InventoryData inventoryData)
        {
            InventoryData = inventoryData;

            if (itemViews == null || itemViews.Length != CellCount)
            {
                itemViews = new GridCell[CellCount];

                for (var i = 0; i < CellCount; i++)
                {
                    var itemView = Instantiate(cellPrefab, gridLayoutGroup.transform).GetComponent<GridCell>();
                    itemViews[i] = itemView;

                    itemView.transform.SetAsFirstSibling();
                    itemView.SetCellEventListener(listener);
                    itemView.Apply(null);
                }

                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = InventoryData.CapacityWidth;
                gridLayoutGroup.cellSize = itemViews.First().CellSize;
                gridLayoutGroup.spacing = Vector2.zero;
            }

            UpdateSize();

            for (var i = 0; i < InventoryData.CellData.Length; i++)
            {
                itemViews[i].Apply(InventoryData.CellData[i]);
            }
        }

        public virtual void OnPrePick(ICell stareCell)
        {
        }

        public virtual bool OnPick(ICell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return false;
            }

            var id = InventoryData.GetId(stareCell.CellData);
            if (id.HasValue)
            {
                originalId = id;
                originalCellData = stareCell.CellData;

                itemViews[id.Value].Apply(null);
                InventoryData.RemoveInventoryItem(id.Value);
                return true;
            }

            return false;
        }

        public virtual void OnDrag(ICell stareCell, ICell effectCell, PointerEventData pointerEventData)
        {
            if (stareCell == null)
            {
                return;
            }

            if (!GetIndex(stareCell).HasValue)
            {
                return;
            }

            // depends on anchor
            var pointerLocalPosition = GetLocalPosition(stareCell.CellRoot, pointerEventData.position, pointerEventData.enterEventCamera);
            var anchor = new Vector2(stareCell.CellSize.x * 0.5f, -stareCell.CellSize.y * 0.5f);
            var anchoredPosition = pointerLocalPosition + anchor;
            CellCornerType = GetCorner((new Vector2(anchoredPosition.x % stareCell.CellSize.x, anchoredPosition.y % stareCell.CellSize.y) - anchor) * 0.5f);
        }

        public virtual bool OnDrop(ICell stareCell, ICell effectCell)
        {
            if (!itemViews.Any(item => item == stareCell))
            {
                return false;
            }

            // check target;
            var index = GetIndex(stareCell, effectCell.CellData, CellCornerType);
            if (!index.HasValue)
            {
                return false;
            }

            if (!InventoryData.CheckInsert(index.Value, effectCell.CellData))
            {
                return false;
            }

            // place
            InventoryData.InsertInventoryItem(index.Value, effectCell.CellData);
            itemViews[index.Value].Apply(effectCell.CellData);

            originalId = null;
            originalCellData = null;
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

        public virtual void OnSwitchRotate(ICell stareCell, ICell effectCell)
        {
        }

        protected virtual int? GetIndex(ICell stareCell)
        {
            var index = (int?)null;
            for (var i = 0; i < itemViews.Length; i++)
            {
                if (itemViews[i] == stareCell)
                {
                    index = i;
                }
            }

            return index;
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

        protected virtual Vector2 GetLocalPosition(RectTransform parent, Vector2 position, Camera camera)
        {
            var localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, camera, out localPosition);
            return localPosition;
        }

        protected virtual CellCornerType GetCorner(Vector2 localPosition)
        {
            // depends on pivot
            var corner = CellCornerType.None;
            if (localPosition.x < Mathf.Epsilon)
            {
                corner |= CellCornerType.Left;
            }

            if (localPosition.x > Mathf.Epsilon)
            {
                corner |= CellCornerType.Right;
            }

            if (localPosition.y > Mathf.Epsilon)
            {
                corner |= CellCornerType.Top;
            }

            if (localPosition.y < Mathf.Epsilon)
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

        void UpdateSize()
        {
            if (itemViews.Any())
            {
                var cellSize = itemViews.First().CellSize;
                resizeTarget.sizeDelta = new Vector2(InventoryData.CapacityWidth * cellSize.x, InventoryData.CapacityHeight * cellSize.y);
            }
        }
    }
}
