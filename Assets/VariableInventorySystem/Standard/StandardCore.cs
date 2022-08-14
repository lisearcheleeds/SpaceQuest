using UnityEngine;

namespace VariableInventorySystem
{
    public class StandardCore : VariableInventoryCore
    {
        [SerializeField] GameObject cellPrefab;
        [SerializeField] RectTransform effectCellParent;

        protected override GameObject CellPrefab => cellPrefab;
        protected override RectTransform EffectCellParent => effectCellParent;
    }
}
