using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardStashView : MonoBehaviour, IVariableInventoryView
    {
        [SerializeField] GridLayoutGroup gridLayoutGroup;
        [SerializeField] Graphic condition;
        [SerializeField] RectTransform resizeTarget;
        [SerializeField] RectTransform conditionTransform;
        [SerializeField] RectTransform background;

        [SerializeField] Color defaultColor;
        [SerializeField] Color positiveColor;
        [SerializeField] Color negativeColor;

        public VariableInventoryViewData StashData { get; private set; }

        public int CellCount => StashData.CapacityWidth * StashData.CapacityHeight;

        protected IVariableInventoryCell[] itemViews;
        protected VariableInventoryCellCorner variableInventoryCellCorner;

        int? originalId;
        IVariableInventoryCellData originalCellData;
        Vector3 conditionOffset;

        GameObject cellPrefab;
        bool isDirty;

        Action<IVariableInventoryCell> onCellClick;
        Action<IVariableInventoryCell> onCellOptionClick;
        Action<IVariableInventoryCell> onCellEnter;
        Action<IVariableInventoryCell> onCellExit;

        public void Initialize(
            GameObject cellPrefab,
            Action<IVariableInventoryCell> onCellClick,
            Action<IVariableInventoryCell> onCellOptionClick,
            Action<IVariableInventoryCell> onCellEnter,
            Action<IVariableInventoryCell> onCellExit)
        {
            this.cellPrefab = cellPrefab;
            this.onCellClick = onCellClick;
            this.onCellOptionClick = onCellOptionClick;
            this.onCellEnter = onCellEnter;
            this.onCellExit = onCellExit;

            condition.raycastTarget = false;
        }

        public virtual void Apply(VariableInventoryViewData stashData)
        {
            StashData = stashData;

            if (itemViews == null || itemViews.Length != CellCount)
            {
                itemViews = new IVariableInventoryCell[CellCount];

                for (var i = 0; i < CellCount; i++)
                {
                    var itemView = Instantiate(cellPrefab, gridLayoutGroup.transform).GetComponent<StandardCell>();
                    itemViews[i] = itemView;

                    itemView.transform.SetAsFirstSibling();
                    itemView.SetCellCallback(
                        onCellClick,
                        onCellOptionClick,
                        onCellEnter,
                        onCellExit);
                    itemView.Apply(null);
                }

                background.SetAsFirstSibling();

                gridLayoutGroup.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
                gridLayoutGroup.constraintCount = StashData.CapacityWidth;
                gridLayoutGroup.cellSize = itemViews.First().CellSize;
                gridLayoutGroup.spacing = Vector2.zero;
            }

            UpdateSize();

            for (var i = 0; i < StashData.CellData.Length; i++)
            {
                itemViews[i].Apply(StashData.CellData[i]);
            }
        }

        public virtual void OnPrePick(IVariableInventoryCell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return;
            }

            var (width, height) = GetRotateSize(stareCell.CellData);
            conditionTransform.sizeDelta = new Vector2(stareCell.CellSize.x * width, stareCell.CellSize.y * height);
        }

        public virtual bool OnPick(IVariableInventoryCell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return false;
            }

            var id = StashData.GetId(stareCell.CellData);
            if (id.HasValue)
            {
                originalId = id;
                originalCellData = stareCell.CellData;

                itemViews[id.Value].Apply(null);
                StashData.RemoveInventoryItem(id.Value);
                isDirty = true;
                return true;
            }

            return false;
        }

        public virtual void OnDrag(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell, PointerEventData pointerEventData)
        {
            if (stareCell == null)
            {
                return;
            }

            if (!GetIndex(stareCell).HasValue)
            {
                UpdateCondition(stareCell, effectCell);
                return;
            }

            // depends on anchor
            var pointerLocalPosition = GetLocalPosition(stareCell.RectTransform, pointerEventData.position, pointerEventData.enterEventCamera);
            var anchor = new Vector2(stareCell.CellSize.x * 0.5f, -stareCell.CellSize.y * 0.5f);
            var anchoredPosition = pointerLocalPosition + anchor;
            conditionOffset = new Vector3(
                Mathf.Floor(anchoredPosition.x / stareCell.CellSize.x) * stareCell.CellSize.x,
                Mathf.Ceil(anchoredPosition.y / stareCell.CellSize.y) * stareCell.CellSize.y);

            // cell corner
            var prevCorner = variableInventoryCellCorner;
            variableInventoryCellCorner = GetCorner((new Vector2(anchoredPosition.x % stareCell.CellSize.x, anchoredPosition.y % stareCell.CellSize.y) - anchor) * 0.5f);

            // shift the position only even number size
            var (width, height) = GetRotateSize(effectCell.CellData);
            var evenNumberOffset = GetEvenNumberOffset(width, height, stareCell.CellSize.x * 0.5f, stareCell.CellSize.y * 0.5f);
            conditionTransform.position = stareCell.RectTransform.position + ((conditionOffset + evenNumberOffset) * stareCell.RectTransform.lossyScale.x);

            // update condition
            if (prevCorner != variableInventoryCellCorner)
            {
                UpdateCondition(stareCell, effectCell);
            }
        }

        public virtual bool OnDrop(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (!itemViews.Any(item => item == stareCell))
            {
                return false;
            }

            // check target;
            var index = GetIndex(stareCell, effectCell.CellData, variableInventoryCellCorner);
            if (!index.HasValue)
            {
                return false;
            }

            if (!StashData.CheckInsert(index.Value, effectCell.CellData))
            {
                return false;
            }

            // place
            StashData.InsertInventoryItem(index.Value, effectCell.CellData);
            isDirty = true;
            itemViews[index.Value].Apply(effectCell.CellData);

            originalId = null;
            originalCellData = null;
            return true;
        }

        public virtual void OnDropped(bool isDropped)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;

            if (!isDropped && originalId.HasValue)
            {
                // revert
                itemViews[originalId.Value].Apply(originalCellData);
                StashData.InsertInventoryItem(originalId.Value, originalCellData);
                isDirty = true;
            }

            originalId = null;
            originalCellData = null;
        }

        public virtual void OnCellEnter(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            conditionTransform.gameObject.SetActive(effectCell?.CellData != null && itemViews.Contains(stareCell));
            (stareCell as StandardCell)?.SetHighLight(true);
        }

        public virtual void OnCellExit(IVariableInventoryCell stareCell)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;

            variableInventoryCellCorner = VariableInventoryCellCorner.None;

            (stareCell as StandardCell)?.SetHighLight(false);
        }

        public virtual void OnSwitchRotate(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (stareCell == null)
            {
                return;
            }

            var (width, height) = GetRotateSize(effectCell.CellData);
            conditionTransform.sizeDelta = new Vector2(effectCell.CellSize.x * width, effectCell.CellSize.y * height);

            var evenNumberOffset = GetEvenNumberOffset(width, height, stareCell.CellSize.x * 0.5f, stareCell.CellSize.y * 0.5f);
            conditionTransform.position = stareCell.RectTransform.position + ((conditionOffset + evenNumberOffset) * stareCell.RectTransform.lossyScale.x);

            UpdateCondition(stareCell, effectCell);
        }

        protected virtual int? GetIndex(IVariableInventoryCell stareCell)
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

        protected virtual int? GetIndex(IVariableInventoryCell stareCell, IVariableInventoryCellData effectCellData, VariableInventoryCellCorner variableInventoryCellCorner)
        {
            var index = GetIndex(stareCell);

            // offset index
            var (width, height) = GetRotateSize(effectCellData);
            if (width % 2 == 0)
            {
                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Left) != VariableInventoryCellCorner.None)
                {
                    index--;
                }
            }

            if (height % 2 == 0)
            {
                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Top) != VariableInventoryCellCorner.None)
                {
                    index -= StashData.CapacityWidth;
                }
            }

            index -= (width - 1) / 2;
            index -= (height - 1) / 2 * StashData.CapacityWidth;
            return index;
        }

        protected virtual (int, int) GetRotateSize(IVariableInventoryCellData cell)
        {
            if (cell == null)
            {
                return (1, 1);
            }

            return (cell.IsRotate ? cell.Height : cell.Width, cell.IsRotate ? cell.Width : cell.Height);
        }

        protected virtual Vector2 GetLocalPosition(RectTransform parent, Vector2 position, Camera camera)
        {
            var localPosition = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, position, camera, out localPosition);
            return localPosition;
        }

        protected virtual VariableInventoryCellCorner GetCorner(Vector2 localPosition)
        {
            // depends on pivot
            var corner = VariableInventoryCellCorner.None;
            if (localPosition.x < Mathf.Epsilon)
            {
                corner |= VariableInventoryCellCorner.Left;
            }

            if (localPosition.x > Mathf.Epsilon)
            {
                corner |= VariableInventoryCellCorner.Right;
            }

            if (localPosition.y > Mathf.Epsilon)
            {
                corner |= VariableInventoryCellCorner.Top;
            }

            if (localPosition.y < Mathf.Epsilon)
            {
                corner |= VariableInventoryCellCorner.Bottom;
            }

            return corner;
        }

        protected virtual Vector3 GetEvenNumberOffset(int width, int height, float widthOffset, float heightOffset)
        {
            var evenNumberOffset = Vector3.zero;

            if (width % 2 == 0)
            {
                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Left) != VariableInventoryCellCorner.None)
                {
                    evenNumberOffset.x -= widthOffset;
                }

                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Right) != VariableInventoryCellCorner.None)
                {
                    evenNumberOffset.x += widthOffset;
                }
            }

            if (height % 2 == 0)
            {
                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Top) != VariableInventoryCellCorner.None)
                {
                    evenNumberOffset.y += heightOffset;
                }

                if ((variableInventoryCellCorner & VariableInventoryCellCorner.Bottom) != VariableInventoryCellCorner.None)
                {
                    evenNumberOffset.y -= heightOffset;
                }
            }

            return evenNumberOffset;
        }

        protected virtual void UpdateCondition(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            var index = GetIndex(stareCell, effectCell.CellData, variableInventoryCellCorner);
            if (index.HasValue && StashData.CheckInsert(index.Value, effectCell.CellData))
            {
                condition.color = positiveColor;
            }
            else
            {
                condition.color = negativeColor;
            }
        }

        void UpdateSize()
        {
            if (itemViews.Any())
            {
                var cellSize = itemViews.First().CellSize;
                resizeTarget.sizeDelta = new Vector2(StashData.CapacityWidth * cellSize.x, StashData.CapacityHeight * cellSize.y);
            }
        }
    }
}
