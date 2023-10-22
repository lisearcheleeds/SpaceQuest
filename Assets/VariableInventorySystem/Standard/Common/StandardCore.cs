using UnityEngine;

namespace VariableInventorySystem
{
    public class StandardCore : Core
    {
        [SerializeField] RectTransform effectCellParent;

        protected override RectTransform EffectCellParent => effectCellParent;
    }
}
