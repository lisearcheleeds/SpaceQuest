using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
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