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
        protected ICell effectCell;

        bool? originalEffectCellRotate;

        public virtual void Initialize()
        {
        }

        public virtual void AddInventoryView(IView view)
        {
            InventoryViews.Add(view);
        }

        public virtual void RemoveInventoryView(IView view)
        {
            InventoryViews.Remove(view);
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

            return true;
        }

        protected virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
            {
                return;
            }

            ICellData cellData = null;
            foreach (var inventoryViews in InventoryViews)
            {
                var id = inventoryViews.GetStareId(eventData);
                if (id.HasValue)
                {
                    cellData = inventoryViews.GetCellData(id.Value);
                    break;
                }
            }

            if (cellData == null)
            {
                return;
            }

            foreach (var inventoryViews in InventoryViews)
            {
                inventoryViews.OnPrePick(cellData);
            }

            var pickInventory = InventoryViews.FirstOrDefault(x => x.OnPick(cellData));
            if (pickInventory == null)
            {
                return;
            }

            effectCell = pickInventory.CreateEffectCell();
            effectCell.CellRoot.SetParent(EffectCellParent, false);
            effectCell.SetClickable(false);
            effectCell.Apply(cellData);
        }

        protected virtual void OnDrag(PointerEventData eventData)
        {
            if (effectCell?.CellData == null)
            {
                return;
            }

            foreach (var inventoryView in InventoryViews)
            {
                inventoryView.OnDrag(effectCell, eventData);
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(EffectCellParent, eventData.position, eventData.enterEventCamera, out var cursorPosition);
            effectCell.SetLocalPosition(cursorPosition);
        }

        protected virtual void OnEndDrag(PointerEventData eventData)
        {
            if (effectCell?.CellData == null)
            {
                return;
            }

            var isRelease = false;
            foreach (var inventoryViews in InventoryViews)
            {
                var id = inventoryViews.GetStareId(eventData);
                if (id.HasValue)
                {
                    isRelease = inventoryViews.OnDrop(id.Value, effectCell.CellData);
                    break;
                }
            }

            if (!isRelease && originalEffectCellRotate.HasValue && effectCell.CellData.IsRotate != originalEffectCellRotate.Value)
            {
                effectCell.SwitchRotate();
            }

            originalEffectCellRotate = null;

            foreach (var inventoryViews in InventoryViews)
            {
                inventoryViews.OnDropped(isRelease);
            }

            Destroy(effectCell.CellRoot.gameObject);
            effectCell = null;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData) => OnBeginDrag(eventData);
        void IDragHandler.OnDrag(PointerEventData eventData) => OnDrag(eventData);
        void IEndDragHandler.OnEndDrag(PointerEventData eventData) => OnEndDrag(eventData);
    }
}
