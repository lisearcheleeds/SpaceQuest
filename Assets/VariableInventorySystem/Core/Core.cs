using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VariableInventorySystem
{
    public abstract class Core : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        protected List<IView> InventoryViews { get; } = new List<IView>();

        protected abstract RectTransform EffectCellParent { get; }

        protected ICell stareCell;
        protected ICell effectCell;

        bool? originalEffectCellRotate;
        Vector2 cursorPosition;

        public virtual void Initialize()
        {
        }

        public virtual void AddInventoryView(IView view)
        {
            InventoryViews.Add(view);
            view.SetCallbacks(OnCellClick, OnCellOptionClick, OnCellEnter, OnCellExit);
        }

        public virtual void RemoveInventoryView(IView view)
        {
            InventoryViews.Remove(view);
        }

        protected virtual void OnCellEnter(ICell cell)
        {
            stareCell = cell;

            foreach (var inventoryView in InventoryViews)
            {
                inventoryView.OnCellEnter(stareCell, effectCell);
            }
        }

        protected virtual void OnCellExit(ICell cell)
        {
            foreach (var inventoryView in InventoryViews)
            {
                inventoryView.OnCellExit(stareCell);
            }

            stareCell = null;
        }

        protected virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            foreach (var inventoryViews in InventoryViews)
            {
                inventoryViews.OnPrePick(stareCell);
            }

            var stareCellData = stareCell.CellData;
            var pickInventory = InventoryViews.FirstOrDefault(x => x.OnPick(stareCell));
            if (pickInventory == null)
            {
                return;
            }

            effectCell = pickInventory.CreateEffectCell();
            effectCell.CellRoot.SetParent(EffectCellParent, false);
            effectCell.SetClickable(false);
            effectCell.Apply(stareCellData);
        }

        protected virtual void OnDrag(PointerEventData eventData)
        {
            if (effectCell?.CellData == null)
            {
                return;
            }

            foreach (var inventoryView in InventoryViews)
            {
                inventoryView.OnDrag(stareCell, effectCell, eventData);
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(EffectCellParent, eventData.position, eventData.enterEventCamera, out cursorPosition);
            effectCell.SetLocalPosition(cursorPosition);
        }

        protected virtual void OnEndDrag(PointerEventData eventData)
        {
            if (effectCell?.CellData == null)
            {
                return;
            }

            var isRelease = InventoryViews.Any(x => x.OnDrop(stareCell, effectCell));

            if (!isRelease && originalEffectCellRotate.HasValue)
            {
                if (effectCell.CellData.IsRotate != originalEffectCellRotate.Value)
                {
                    effectCell.SwitchRotate();
                }
            }

            originalEffectCellRotate = null;

            foreach (var inventoryViews in InventoryViews)
            {
                inventoryViews.OnDropped(isRelease);
            }

            Destroy(effectCell.CellRoot.gameObject);
            effectCell = null;
        }

        public virtual bool SwitchRotate()
        {
            if (effectCell?.CellData == null)
            {
                return false;
            }

            if (!originalEffectCellRotate.HasValue)
            {
                originalEffectCellRotate = effectCell.CellData.IsRotate;
            }

            var isValid = effectCell.SwitchRotate();

            if (!isValid)
            {
                originalEffectCellRotate = null;
                return false;
            }

            foreach (var inventoryViews in InventoryViews)
            {
                inventoryViews.OnSwitchRotate(stareCell, effectCell);
            }

            return true;
        }

        protected virtual void OnCellClick(ICell cell)
        {
        }

        protected virtual void OnCellOptionClick(ICell cell)
        {
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }
    }
}
