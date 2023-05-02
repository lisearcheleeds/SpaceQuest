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
        ActorData actorData;
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
            MessageBus.Instance.SetUserPlayer.AddListener(SetUserPlayer);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);
        }
        
        public void Finalize()
        {
            MessageBus.Instance.SetUserPlayer.RemoveListener(SetUserPlayer);
            MessageBus.Instance.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);
        }

        void Update()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
            }
        }

        void SetUserPlayer(PlayerQuestData playerQuestData)
        {
            actorData = playerQuestData.MainActorData;
            isDirty = true;
        }

        void UserInputSetCurrentWeaponGroupIndex(int index)
        {
            isDirty = true;
        }

        void RefreshWeaponDataView()
        {
            for (var i = 0; i < weaponGroupTabList.Length; i++)
            {
                weaponGroupTabList[i].SetActiveColor(i == actorData.ActorStateData.CurrentWeaponGroupIndex);
            }

            var currentWeaponDataGroup = actorData.WeaponDataGroup[actorData.ActorStateData.CurrentWeaponGroupIndex];
            var loopMax = Mathf.Max(weaponDataViewList.Count, currentWeaponDataGroup.Count);
            for (var i = 0; i < loopMax; i++)
            {
                if (weaponDataViewList.Count <= i)
                {
                    weaponDataViewList.Add(Instantiate(weaponDataViewPrefab, weaponDataViewParent));
                }

                if (i < currentWeaponDataGroup.Count)
                {
                    weaponDataViewList[i].SetWeaponData(actorData.WeaponData[currentWeaponDataGroup[i]]);
                }
                else
                {
                    weaponDataViewList[i].SetWeaponData(null);
                }
            }
        }
    }
}