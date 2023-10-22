using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace AloneSpace.Common
{
    public class InventoryCell : StandardGridCell
    {
        [SerializeField] Text text;

        protected override void OnApply()
        {
            base.OnApply();

            if (CellData is ItemData cellData)
            {
                text.gameObject.SetActive(true);
                text.text = cellData.ItemVO.Text;
            }
            else
            {
                text.gameObject.SetActive(false);
            }
        }
    }
}
