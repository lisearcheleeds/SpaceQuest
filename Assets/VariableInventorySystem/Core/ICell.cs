using UnityEngine;

namespace VariableInventorySystem
{
    public interface ICell
    {
        RectTransform CellRoot { get; }
        ICellData CellData { get; }

        Vector2 CellSize { get; }

        void Apply(ICellData data);

        void SetLocalPosition(Vector2 localPosition);
        bool SwitchRotate();
        void SetClickable(bool clickable);
        Vector3 GetRotateSize(bool isRotate);
    }
}
