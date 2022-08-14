using UnityEngine;

namespace VariableInventorySystem
{
    public interface IVariableInventoryCell
    {
        RectTransform RectTransform { get; }
        IVariableInventoryCellData CellData { get; }
        Vector2 CellSize { get; }

        void Apply(IVariableInventoryCellData data);

        void SetSelectable(bool isSelectable);
    }
}