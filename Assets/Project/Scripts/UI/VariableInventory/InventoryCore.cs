using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryCore : Core
    {
        [SerializeField] RectTransform effectCellParent;

        protected override RectTransform EffectCellParent => effectCellParent;
    }
}
