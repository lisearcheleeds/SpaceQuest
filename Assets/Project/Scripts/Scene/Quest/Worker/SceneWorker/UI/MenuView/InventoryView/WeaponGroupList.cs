using System;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponGroupList : MonoBehaviour
    {
        [SerializeField] WeaponGroupListView weaponGroupListView;

        QuestData questData;
        WeaponGroupListViewCell.CellData[] weaponGroupListViewCellDataList;

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
                weaponGroupListViewCellDataList = Array.Empty<WeaponGroupListViewCell.CellData>();
            }
            else
            {
                weaponGroupListViewCellDataList = currentControlActorData.WeaponSpecVOs.Select(x => new WeaponGroupListViewCell.CellData(x)).ToArray();
            }

            weaponGroupListView.Apply(weaponGroupListViewCellDataList);
        }
    }
}
