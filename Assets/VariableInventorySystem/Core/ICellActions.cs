using System;

namespace VariableInventorySystem
{
    public interface ICellActions
    {
        bool IsActive { get; set; }
        void SetCallback(
            Action onPointerClick,
            Action onPointerClickOption,
            Action onPointerEnter,
            Action onPointerExit);
    }
}
