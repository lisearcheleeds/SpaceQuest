using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace.Common
{
    public class DropAreaCell : VariableInventoryCell
    {
        public override Vector2 CellSize { get; }

        protected override IVariableInventoryCellActions ButtonActions => button;

        [SerializeField] StandardButton button;
        [SerializeField] Graphic raycastTarget;

        public override void SetSelectable(bool value)
        {
            raycastTarget.raycastTarget = value;
        }

        protected override void OnApply()
        {
        }
    }
}
