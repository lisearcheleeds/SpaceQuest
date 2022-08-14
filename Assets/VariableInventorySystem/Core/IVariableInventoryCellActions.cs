using System;

namespace VariableInventorySystem
{
    public interface IVariableInventoryCellActions
    {
        bool IsActive { get; set; }
        void SetCallback(
            Action onPointerClick,
            Action onPointerClickOption,
            Action onPointerEnter,
            Action onPointerExit);
    }
}