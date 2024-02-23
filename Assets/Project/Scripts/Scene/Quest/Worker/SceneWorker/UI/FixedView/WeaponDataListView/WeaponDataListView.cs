using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace.UI
{
    public class WeaponDataListView : MonoBehaviour
    {
        [SerializeField] Animator listAnimator;
        [SerializeField] RectTransform weaponGroupTabParent;
        [SerializeField] RectTransform weaponDataViewParent;
        [SerializeField] WeaponGroupTab weaponGroupTabPrefab;
        [SerializeField] WeaponDataView weaponDataViewPrefab;

        List<WeaponGroupTab> weaponGroupTabs = new List<WeaponGroupTab>();
        List<WeaponDataView> weaponDataViews = new List<WeaponDataView>();

        ActorData userControlActor;
        bool isDirty;

        public void Initialize()
        {
            MessageBus.Instance.User.SetControlActor.AddListener(SetUserControlActor);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.AddListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetControlActor.RemoveListener(SetUserControlActor);
            MessageBus.Instance.UserInput.UserInputSetCurrentWeaponGroupIndex.RemoveListener(UserInputSetCurrentWeaponGroupIndex);
        }

        public void OnUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                RefreshWeaponDataView();
                
                listAnimator.SetTrigger(AnimatorKey.Play);
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
                foreach (var weaponGroupTab in weaponGroupTabs)
                {
                    weaponGroupTab.gameObject.SetActive(false);
                }

                foreach (var weaponDataView in weaponDataViews)
                {
                    weaponDataView.gameObject.SetActive(false);
                }

                return;
            }

            // WeaponGroup
            var weaponDataGroupCount = userControlActor.WeaponDataGroup.Count(x => x.Any());
            for (var i = 0; i < Mathf.Max(weaponGroupTabs.Count, weaponDataGroupCount); i++)
            {
                if (weaponGroupTabs.Count <= i)
                {
                    weaponGroupTabs.Add(Instantiate(weaponGroupTabPrefab, weaponGroupTabParent));
                    weaponGroupTabs[i].SetIndex(i);
                }
                
                weaponGroupTabs[i].gameObject.SetActive(i < weaponDataGroupCount);
                weaponGroupTabs[i].SetActiveColor(i == userControlActor.ActorStateData.CurrentWeaponGroupIndex);
            }

            // WeaponData
            var currentWeaponDataList = userControlActor.WeaponDataGroup[userControlActor.ActorStateData.CurrentWeaponGroupIndex]
                .Select(x => userControlActor.WeaponData[x])
                .ToArray();
            
            for (var i = 0; i < Mathf.Max(weaponDataViews.Count, currentWeaponDataList.Count()); i++)
            {
                if (weaponDataViews.Count <= i)
                {
                    weaponDataViews.Add(Instantiate(weaponDataViewPrefab, weaponDataViewParent));
                }
                
                weaponDataViews[i].SetWeaponData(i < currentWeaponDataList.Count() ? currentWeaponDataList[i] : null);
            }
        }
    }
}
