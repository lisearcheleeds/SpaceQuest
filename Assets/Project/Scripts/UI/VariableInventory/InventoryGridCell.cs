using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryGridCell : GridCell<ItemData>, ICellActions
    {
        protected override ICellActions CellActions => this;

        [SerializeField] ItemThumbnail itemThumbnail;

        protected override void OnApply()
        {
            base.OnApply();

            itemThumbnail.Apply(
                GridCellData,
                layoutMode: ItemThumbnail.LayoutMode.Manual,
                pointerActionMode: ItemThumbnail.PointerActionMode.Manual);
            itemThumbnail.SetPointerAction(false);
            itemThumbnail.SetLayout(true, true, false);
        }

        public override void SetClickable(bool clickable)
        {
            itemThumbnail.StandardButton.IsActive = clickable;
        }

        void ICellActions.SetCallback(Action onClick, Action onClickOption, Action onEnter, Action onExit)
        {
            itemThumbnail.SetCallBack(onClick, onClickOption, onEnter, onExit);
        }
    }
}
