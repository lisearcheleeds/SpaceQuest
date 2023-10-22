using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VariableInventorySystem
{
    public interface IView
    {
        ICell CreateEffectCell();

        void SetCellEventListener(ICellEventListener listener);

        void OnPrePick(ICell stareCell);
        bool OnPick(ICell stareCell);
        void OnDrag(ICell stareCell, ICell effectCell, PointerEventData cursorPosition);
        bool OnDrop(ICell stareCell, ICell effectCell);
        void OnDropped(bool isDropped);

        void OnCellEnter(ICell stareCell, ICell effectCell);
        void OnCellExit(ICell stareCell);

        void OnSwitchRotate(ICell stareCell, ICell effectCell);
    }
}
