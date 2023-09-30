using UnityEngine;
using VariableInventorySystem;

namespace AloneSpace
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] WeaponList weaponList;
        [SerializeField] InventoryList inventoryList;
        [SerializeField] InAreaItemList inAreaItemList;

        [SerializeField] StandardCore standardCore;

        public void Initialize(QuestData questData)
        {
            standardCore.Initialize();
            standardCore.AddInventoryView(inventoryList.StandardStashView);

            weaponList.Initialize(questData);
            inventoryList.Initialize(questData);
            inAreaItemList.Initialize(questData);

        }

        public void Finalize()
        {
            weaponList.Finalize();
            inventoryList.Finalize();
            inAreaItemList.Finalize();
        }

        public void OnUpdate()
        {
            weaponList.OnUpdate();
        }
    }
}
