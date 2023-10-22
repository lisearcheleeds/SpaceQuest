using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardGridView : GridView
    {
        [SerializeField] RectTransform background;

        [SerializeField] Graphic condition;
        [SerializeField] RectTransform conditionTransform;

        [SerializeField] Color defaultColor;
        [SerializeField] Color positiveColor;
        [SerializeField] Color negativeColor;

        Vector3 conditionOffset;

        public override void Initialize()
        {
            base.Initialize();

            condition.raycastTarget = false;
        }

        public override void Apply(InventoryData stashData)
        {
            base.Apply(stashData);

            background.SetAsFirstSibling();
        }

        public override void OnPrePick(ICell stareCell)
        {
            base.OnPrePick(stareCell);

            if (stareCell?.CellData == null)
            {
                return ;
            }

            var rotateSize = stareCell.GetRotateSize(stareCell.CellData.IsRotate);
            conditionTransform.sizeDelta = rotateSize;
        }

        public override void OnDrag(ICell stareCell, ICell effectCell, PointerEventData pointerEventData)
        {
            var prevCorner = CellCornerType;

            base.OnDrag(stareCell, effectCell, pointerEventData);

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
            var pointerLocalPosition = GetLocalPosition(stareCell.CellRoot, pointerEventData.position, pointerEventData.enterEventCamera);
            var anchor = new Vector2(stareCell.CellSize.x * 0.5f, -stareCell.CellSize.y * 0.5f);
            var anchoredPosition = pointerLocalPosition + anchor;
            conditionOffset = new Vector3(
                Mathf.Floor(anchoredPosition.x / stareCell.CellSize.x) * stareCell.CellSize.x,
                Mathf.Ceil(anchoredPosition.y / stareCell.CellSize.y) * stareCell.CellSize.y);

            // shift the position only even number size
            var (widthCount, heightCount) = GridLayoutHelper.GetRotateDataSize(effectCell.CellData);
            var evenNumberOffset = GetEvenNumberOffset(widthCount, heightCount, stareCell.CellSize.x * 0.5f, stareCell.CellSize.y * 0.5f);
            conditionTransform.position = stareCell.CellRoot.position + ((conditionOffset + evenNumberOffset) * stareCell.CellRoot.lossyScale.x);

            // update condition
            if (prevCorner != CellCornerType)
            {
                UpdateCondition(stareCell, effectCell);
            }
        }

        public override void OnDropped(bool isDropped)
        {
            base.OnDropped(isDropped);

            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public override void OnCellEnter(ICell stareCell, ICell effectCell)
        {
            base.OnCellEnter(stareCell, effectCell);

            conditionTransform.gameObject.SetActive(effectCell?.CellData != null && itemViews.Contains(stareCell));
        }

        public override void OnCellExit(ICell stareCell)
        {
            base.OnCellExit(stareCell);

            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public override void OnSwitchRotate(ICell stareCell, ICell effectCell)
        {
            base.OnSwitchRotate(stareCell, effectCell);

            if (stareCell == null)
            {
                return;
            }

            var (widthCount, heightCount) = GridLayoutHelper.GetRotateDataSize(effectCell.CellData);
            conditionTransform.sizeDelta = new Vector2(effectCell.CellSize.x * widthCount, effectCell.CellSize.y * heightCount);

            var evenNumberOffset = GetEvenNumberOffset(widthCount, heightCount, stareCell.CellSize.x * 0.5f, stareCell.CellSize.y * 0.5f);
            conditionTransform.position = stareCell.CellRoot.position + ((conditionOffset + evenNumberOffset) * stareCell.CellRoot.lossyScale.x);

            UpdateCondition(stareCell, effectCell);
        }

        protected virtual void UpdateCondition(ICell stareCell, ICell effectCell)
        {
            var index = GetIndex(stareCell, effectCell.CellData, CellCornerType);
            if (index.HasValue && InventoryData.CheckInsert(index.Value, effectCell.CellData))
            {
                condition.color = positiveColor;
            }
            else
            {
                condition.color = negativeColor;
            }
        }
    }
}
