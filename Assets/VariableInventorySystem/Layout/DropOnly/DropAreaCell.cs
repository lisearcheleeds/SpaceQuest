using UnityEngine;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class DropAreaCell : Cell
    {
        public override Vector2 CellSize { get; }
        protected override RectTransform SizeRoot => null;
        protected override RectTransform RotateRoot => null;

        protected override ICellActions CellActions => button;

        [SerializeField] StandardButton button;
        [SerializeField] Graphic raycastTarget;

        public override void SetClickable(bool clickable)
        {
            raycastTarget.raycastTarget = clickable;
        }

        protected override void OnApply()
        {
        }
    }
}
