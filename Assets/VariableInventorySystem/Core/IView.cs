using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VariableInventorySystem
{
    public interface IView
    {
        ICell CreateEffectCell();

        int? GetStareId(PointerEventData cursorPosition);
        ICellData GetCellData(int id);

        void OnPrePick(ICellData cellData);
        bool OnPick(ICellData cellData);
        void OnDrag(ICell effectCell, PointerEventData cursorPosition);
        bool OnDrop(int? dropTargetId, ICellData cellData);
        void OnDropped(bool isDropped);
    }
}
