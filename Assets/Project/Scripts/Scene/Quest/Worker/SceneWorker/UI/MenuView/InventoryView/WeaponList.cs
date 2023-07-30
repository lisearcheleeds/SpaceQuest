using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponList : MonoBehaviour
    {
        [SerializeField] WeaponListView weaponListView;

        QuestData questData;
        WeaponListViewCell.CellData[] weaponListViewCellDataList;

        bool isDirty;
        ActorData currentControlActorData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }

        public void OnUpdate()
        {
            if (questData.UserData.ControlActorData?.InstanceId != currentControlActorData?.InstanceId)
            {
                isDirty = true;
            }

            if (!isDirty || !enabled)
            {
                return;
            }

            isDirty = false;
            Refresh();
        }

        void Refresh()
        {
            currentControlActorData = questData.UserData.ControlActorData;

            if (currentControlActorData == null)
            {
                weaponListViewCellDataList = Array.Empty<WeaponListViewCell.CellData>();
            }
            else
            {
                weaponListViewCellDataList = currentControlActorData.WeaponSpecVOs.Select(x => new WeaponListViewCell.CellData(x)).ToArray();
            }

            weaponListView.Apply(weaponListViewCellDataList);
        }
    }
}
