using UnityEngine;
using VariableInventorySystem;

namespace RoboQuest.Quest
{
    public class InventoryDataView : MonoBehaviour
    {
        public StandardStashView StandardStashView => standardStashView;
        
        [SerializeField] StandardStashView standardStashView;

        public void Apply(InventoryData inventoryData)
        {
            standardStashView.Apply(inventoryData.VariableInventoryViewData);
        }
    }
}