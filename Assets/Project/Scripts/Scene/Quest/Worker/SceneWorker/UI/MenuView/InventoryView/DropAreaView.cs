using System;
using AloneSpace.Common;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace
{
    /// <summary>
    /// Drag and DropのDrop専用View
    /// </summary>
    public class DropAreaView : MonoBehaviour, IVariableInventoryView
    {
        [SerializeField] DropAreaCell dropAreaCell;
        [SerializeField] Graphic condition;
        [SerializeField] RectTransform conditionTransform;

        [SerializeField] Color defaultColor;
        [SerializeField] Color positiveColor;
        [SerializeField] Color negativeColor;

        Func<IVariableInventoryCellData, bool> onDrop;
        Func<IVariableInventoryCellData, bool> getIsInsertableCondition;
        Func<IVariableInventoryCellData, bool> getIsInnerCell;

        public void Initialize(
            GameObject cellPrefab,
            Action<IVariableInventoryCell> onCellClick,
            Action<IVariableInventoryCell> onCellOptionClick,
            Action<IVariableInventoryCell> onCellEnter,
            Action<IVariableInventoryCell> onCellExit)
        {
            dropAreaCell.SetCellCallback(onCellClick, onCellOptionClick, onCellEnter, onCellExit);

            dropAreaCell.SetSelectable(false);
        }

        public virtual void Apply(
            Func<IVariableInventoryCellData, bool> onDrop,
            Func<IVariableInventoryCellData, bool> getIsInsertableCondition,
            Func<IVariableInventoryCellData, bool> getIsInnerCell)
        {
            this.onDrop = onDrop;
            this.getIsInsertableCondition = getIsInsertableCondition;
            this.getIsInnerCell = getIsInnerCell;
        }

        public virtual void OnPrePick(IVariableInventoryCell stareCell)
        {
            dropAreaCell.SetSelectable(!getIsInnerCell(stareCell.CellData));
        }

        public virtual bool OnPick(IVariableInventoryCell stareCell)
        {
            return false;
        }

        public virtual void OnDrag(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell, PointerEventData pointerEventData)
        {
            if (stareCell == null)
            {
                return;
            }

            UpdateCondition(stareCell, effectCell);
        }

        public virtual bool OnDrop(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (stareCell != dropAreaCell)
            {
                return false;
            }

            return onDrop(effectCell.CellData);
        }

        public virtual void OnDropped(bool isDropped)
        {
            dropAreaCell.SetSelectable(false);

            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public virtual void OnCellEnter(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            conditionTransform.gameObject.SetActive(effectCell?.CellData != null && dropAreaCell == stareCell);
        }

        public virtual void OnCellExit(IVariableInventoryCell stareCell)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public virtual void OnSwitchRotate(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
        }

        protected virtual void UpdateCondition(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell)
        {
            if (stareCell == dropAreaCell && getIsInsertableCondition(effectCell.CellData))
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
