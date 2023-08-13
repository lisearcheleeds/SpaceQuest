using AloneSpace.Common;
using UnityEngine;

namespace AloneSpace
{
    public class InventoryView : MonoBehaviour
    {
        [SerializeField] WeaponList weaponList;

        public void Initialize(QuestData questData)
        {
            weaponList.Initialize(questData);
        }

        public void Finalize()
        {
            weaponList.Finalize();
        }

        public void OnUpdate()
        {
            weaponList.OnUpdate();
        }
    }
}
