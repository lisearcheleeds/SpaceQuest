using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    /// <summary>
    /// Drag and DropのDrop専用View
    /// </summary>
    public class DropAreaView : MonoBehaviour, IView
    {
        [SerializeField] DropAreaCell dropAreaCell;
        [SerializeField] Graphic condition;
        [SerializeField] RectTransform conditionTransform;

        [SerializeField] Color defaultColor;
        [SerializeField] Color positiveColor;
        [SerializeField] Color negativeColor;

        Func<ICellData, bool> onDrop;
        Func<ICellData, bool> getIsInsertableCondition;
        Func<ICellData, bool> getIsInnerCell;
        IView viewImplementation;

        public ICell CreateEffectCell()
        {
            throw new NotSupportedException("This method is only used for Drop, so it will not be called if the operation is normal.");
        }

        public void SetCellEventListener(ICellEventListener listener)
        {
            dropAreaCell.SetCellEventListener(listener);
            dropAreaCell.SetClickable(false);
        }

        public virtual void Apply(
            Func<ICellData, bool> onDrop,
            Func<ICellData, bool> getIsInsertableCondition,
            Func<ICellData, bool> getIsInnerCell)
        {
            this.onDrop = onDrop;
            this.getIsInsertableCondition = getIsInsertableCondition;
            this.getIsInnerCell = getIsInnerCell;
        }

        public virtual void OnPrePick(ICell stareCell)
        {
            if (stareCell?.CellData == null)
            {
                return;
            }

            dropAreaCell.SetClickable(!getIsInnerCell(stareCell.CellData));
        }

        public virtual bool OnPick(ICell stareCell)
        {
            return false;
        }

        public virtual void OnDrag(ICell stareCell, ICell effectCell, PointerEventData pointerEventData)
        {
            if (stareCell?.CellData == null)
            {
                return;
            }

            UpdateCondition(stareCell, effectCell);
        }

        public virtual bool OnDrop(ICell stareCell, ICell effectCell)
        {
            if (stareCell != dropAreaCell)
            {
                return false;
            }

            return onDrop(effectCell.CellData);
        }

        public virtual void OnDropped(bool isDropped)
        {
            dropAreaCell.SetClickable(false);

            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public virtual void OnCellEnter(ICell stareCell, ICell effectCell)
        {
            conditionTransform.gameObject.SetActive(effectCell?.CellData != null && dropAreaCell == stareCell);
        }

        public virtual void OnCellExit(ICell stareCell)
        {
            conditionTransform.gameObject.SetActive(false);
            condition.color = defaultColor;
        }

        public virtual void OnSwitchRotate(ICell stareCell, ICell effectCell)
        {
        }

        protected virtual void UpdateCondition(ICell stareCell, ICell effectCell)
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
