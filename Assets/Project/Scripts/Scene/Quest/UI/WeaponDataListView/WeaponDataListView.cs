using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class WeaponDataListView : MonoBehaviour
    {
        [SerializeField] WeaponDataView weaponDataViewPrefab;
        [SerializeField] RectTransform weaponDataViewParent;

        [SerializeField] WeaponGroupTab[] weaponGroupTabList;

        List<WeaponDataView> weaponDataViewList = new List<WeaponDataView>();
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
            MessageBus.Instance.SetUserControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
            }

            foreach (var weaponDataView in weaponDataViewList)
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

                foreach (var weaponDataView in weaponDataViewList)
                {
                    weaponDataView.SetWeaponData(null);
                }

                return;
            }

            for (var i = 0; i < weaponGroupTabList.Length; i++)
            {
                weaponGroupTabList[i].SetActiveColor(i == userControlActor.ActorStateData.CurrentWeaponGroupIndex);
            }

            var currentWeaponDataGroup = userControlActor.WeaponDataGroup[userControlActor.ActorStateData.CurrentWeaponGroupIndex];
            var loopMax = Mathf.Max(weaponDataViewList.Count, currentWeaponDataGroup.Count);
            for (var i = 0; i < loopMax; i++)
            {
                if (weaponDataViewList.Count <= i)
                {
                    weaponDataViewList.Add(Instantiate(weaponDataViewPrefab, weaponDataViewParent));
                }

                if (i < currentWeaponDataGroup.Count)
                {
                    weaponDataViewList[i].SetWeaponData(userControlActor.WeaponData[currentWeaponDataGroup[i]]);
                }
                else
                {
                    weaponDataViewList[i].SetWeaponData(null);
                }
            }
        }
    }
}
