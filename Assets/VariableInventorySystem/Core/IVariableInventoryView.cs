﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace VariableInventorySystem
{
    public interface IVariableInventoryView
    {
        void Initialize(
            GameObject cellPrefab, 
            Action<IVariableInventoryCell> onCellClick,
            Action<IVariableInventoryCell> onCellOptionClick,
            Action<IVariableInventoryCell> onCellEnter,
            Action<IVariableInventoryCell> onCellExit);

        void Apply(VariableInventoryViewData data);
        void ReApply();

        void OnPrePick(IVariableInventoryCell stareCell);
        bool OnPick(IVariableInventoryCell stareCell);
        void OnDrag(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell, PointerEventData cursorPosition);
        bool OnDrop(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell);
        void OnDroped(bool isDroped);

        void OnCellEnter(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell);
        void OnCellExit(IVariableInventoryCell stareCell);

        void OnSwitchRotate(IVariableInventoryCell stareCell, IVariableInventoryCell effectCell);
    }
}
