using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class WeaponDataListView : MonoBehaviour
    {
        [SerializeField] WeaponGroupTab[] weaponGroupTabList;
        [SerializeField] WeaponDataView[] weaponDataViews;

        ActorData userControlActor;
        bool isDirty;

        [Serializable]
        public class WeaponGroupTab
        {
            [SerializeField] Image line;
            [SerializeField] Text text;

            [SerializeField] Color activeLineColor;
            [SerializeField] Color disableLineColor;
            [SerializeField] Color activeTextColor;
            [SerializeField] Color disableTextColor;

            public void SetActiveColor(bool isActive)
            {
                line.color = isActive ? activeLineColor : disableLineColor;
                text.color = isActive ? activeTextColor : disableTextColor;
            }
        }

        public void Initialize()
        {
            MessageBus.Instance.Temp.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void Finalize()
        {
            MessageBus.Instance.Temp.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
            }

            foreach (var weaponDataView in weaponDataViews)
            {
                weaponDataView.OnUpdate();
            }
        }

        void SetUserControlActor(ActorData userControlActor)
        {
            this.userControlActor = userControlActor;
            isDirty = true;
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            isDirty = true;
        }

        void RefreshWeaponDataView()
        {
            if (userControlActor == null)
            {
                foreach (var weaponGroupTab in weaponGroupTabList)
                {
                    weaponGroupTab.SetActiveColor(false);
                }

                foreach (var weaponDataView in weaponDataViews)
                {
                    weaponDataView.SetWeaponDataList(null);
                }

                return;
            }

            for (var i = 0; i < weaponGroupTabList.Length; i++)
            {
                weaponGroupTabList[i].SetActiveColor(i == userControlActor.ActorStateData.CurrentWeaponGroupIndex);
            }

            // TODO: 文字列でGroupByしてるが何か良い方法を探す
            var currentWeaponDataGroup = userControlActor.WeaponDataGroup[userControlActor.ActorStateData.CurrentWeaponGroupIndex]
                .GroupBy(id => $"{userControlActor.WeaponData[id].WeaponSpecVO.WeaponType}:{userControlActor.WeaponData[id].WeaponSpecVO.Id}")
                .Select(x => x.Select(y => userControlActor.WeaponData[y]).ToArray())
                .ToArray();

            for (var i = 0; i < weaponDataViews.Length; i++)
            {
                weaponDataViews[i].SetWeaponDataList(
                    i < currentWeaponDataGroup.Length
                    ? currentWeaponDataGroup[i]
                    : null);
            }
        }
    }
}
