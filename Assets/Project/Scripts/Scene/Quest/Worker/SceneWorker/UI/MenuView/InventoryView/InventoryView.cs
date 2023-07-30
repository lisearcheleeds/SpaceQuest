using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] TabController tabController;

        [SerializeField] WeaponList weaponList;
        [SerializeField] WeaponGroupList weaponGroupList;

        public void Initialize(QuestData questData)
        {
            weaponList.Initialize(questData);
            weaponGroupList.Initialize(questData);
        }

        public void Finalize()
        {
            weaponList.Finalize();
            weaponGroupList.Finalize();
        }

        public void OnUpdate()
        {
            weaponList.OnUpdate();
            weaponGroupList.OnUpdate();
        }
    }
}
