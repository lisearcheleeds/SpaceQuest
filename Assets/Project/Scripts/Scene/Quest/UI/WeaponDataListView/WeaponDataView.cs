using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class WeaponDataView : MonoBehaviour
    {
        [SerializeField] Image weaponImage;
        [SerializeField] Text currentWeaponEffectCount;
        [SerializeField] GameObject hitEffect;
        [SerializeField] Text hitCount;
        [SerializeField] GameObject executeImage;
        [SerializeField] RectTransform resourceGaugeRect;
        [SerializeField] RectTransform reloadGaugeRect;
        [SerializeField] Text resourceValue;
        [SerializeField] Text resourceMax;

        WeaponData weaponData;

        int prevWeaponEffectCount;
        int prevResourceValue;
        bool prevReloadValueIsZero;
        
        public void SetWeaponData(WeaponData weaponData)
        {
            this.weaponData = weaponData;
            gameObject.SetActive(weaponData != null);

            if (weaponData == null)
            {
                return;
            }

            prevResourceValue = -1;
            prevReloadValueIsZero = false;
            prevWeaponEffectCount = -1;
            resourceMax.text = $"/{weaponData.WeaponSpecVO.WeaponResourceMaxCount}";
        }

        public void OnLateUpdate()
        {
            if (weaponData == null)
            {
                return;
            }
            
            executeImage.gameObject.SetActive(weaponData.WeaponStateData.IsExecute);

            var weaponEffectCount = weaponData.WeaponStateData.WeaponEffectDataList.Count;
            if (prevWeaponEffectCount != weaponEffectCount)
            {
                prevWeaponEffectCount = weaponEffectCount;

                currentWeaponEffectCount.text = $"{weaponEffectCount}";
            }

            if (prevResourceValue != weaponData.WeaponStateData.ResourceIndex)
            {
                prevResourceValue = weaponData.WeaponStateData.ResourceIndex;
                
                var resourceRemainCount = weaponData.WeaponSpecVO.WeaponResourceMaxCount - weaponData.WeaponStateData.ResourceIndex;
                resourceValue.text = resourceRemainCount.ToString();

                var resourceRatio = (float)resourceRemainCount / weaponData.WeaponSpecVO.WeaponResourceMaxCount;
                var resourceGaugeRectLocalScale = resourceGaugeRect.localScale;
                resourceGaugeRectLocalScale.x = resourceRatio;
                resourceGaugeRect.localScale = resourceGaugeRectLocalScale;
            }

            if (!prevReloadValueIsZero || 0 != weaponData.WeaponStateData.ReloadRemainTime)
            {
                prevReloadValueIsZero = weaponData.WeaponStateData.ReloadRemainTime == 0;
                var reloadRatio = prevReloadValueIsZero ? 0.0f : 1.0f - (weaponData.WeaponStateData.ReloadRemainTime / weaponData.WeaponSpecVO.ReloadTime);
                var reloadGaugeRectLocalScale = reloadGaugeRect.localScale;
                reloadGaugeRectLocalScale.x = reloadRatio;
                reloadGaugeRect.localScale = reloadGaugeRectLocalScale;
            }
        }
    }
}