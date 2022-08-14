using UnityEngine;
using UnityEngine.UI;
using VariableInventorySystem;

namespace RoboQuest.Quest
{
    public class ActorInventoryCell : StandardCell
    {
        [SerializeField] Text amountText;
        
        protected override void OnApply()
        {
            base.OnApply();

            if (CellData == null)
            {
                amountText.gameObject.SetActive(false);
                return;
            }

            var itemData = (ItemData)CellData;
            amountText.gameObject.SetActive(itemData.HasAmount);
            if (itemData.HasAmount)
            {
                amountText.text = itemData.Amount.ToString();
            }
        }
    }
}