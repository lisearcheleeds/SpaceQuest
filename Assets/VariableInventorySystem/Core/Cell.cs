using System;
using UnityEngine;

namespace VariableInventorySystem
{
    public abstract class Cell : MonoBehaviour, ICell
    {
        public RectTransform CellRoot => (RectTransform)transform;

        public ICellData CellData { get; private set; }

        public abstract Vector2 CellSize { get; }

        protected abstract RectTransform SizeRoot { get; }
        protected abstract RectTransform RotateRoot { get; }

        public void Apply(ICellData cellData)
        {
            CellData = cellData;
            OnApply();
        }

        public virtual void SetLocalPosition(Vector2 localPosition)
        {
            CellRoot.localPosition = localPosition + GetCenterOffset();
        }

        public virtual bool SwitchRotate()
        {
            var prevRotateSize = GetRotateSize(CellData.IsRotate);

            CellData.IsRotate = !CellData.IsRotate;
            Apply(CellData);

            var nextRotateSize = GetRotateSize(CellData.IsRotate);

            CellRoot.localPosition = CellRoot.localPosition - prevRotateSize + nextRotateSize;

            SizeRoot.sizeDelta = nextRotateSize;
            RotateRoot.sizeDelta = CellSize;
            RotateRoot.localEulerAngles = Vector3.forward * (CellData?.IsRotate ?? false ? 90 : 0);

            return true;
        }

        public abstract void SetClickable(bool clickable);

        public virtual Vector3 GetRotateSize(bool isRotate)
        {
            if (!isRotate)
            {
                return CellSize;
            }

            return new Vector3(CellSize.y, CellSize.x, 0);
        }

        protected abstract void OnApply();

        protected virtual Vector2 GetCenterOffset()
        {
            var rotateSize = GetRotateSize(CellData.IsRotate);
            return new Vector2(-rotateSize.x * 0.5f, rotateSize.y * 0.5f);
        }
    }
}
