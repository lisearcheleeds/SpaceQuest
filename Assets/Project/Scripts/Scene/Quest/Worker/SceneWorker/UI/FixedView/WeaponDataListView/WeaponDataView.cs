using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class WeaponDataView : MonoBehaviour
    {
        [SerializeField] Animator hitAnimator;

        [SerializeField] Text name;
        [SerializeField] Image singleWeaponIcon;
        [SerializeField] RectTransform singleReloadGaugeRect;
        [SerializeField] Text resourceCount;

        [SerializeField] Color executableColor;
        [SerializeField] Color executingColor;
        [SerializeField] Color reloadingColor;
        [SerializeField] Color resourceEmptyColor;
        [SerializeField] Color disableColor;

        WeaponData weaponData;

        int prevTotalCollideCount;
        int prevResourceValue;
        bool prevReloadValueIsZero;

        public void SetWeaponData(WeaponData weaponData)
        {
            gameObject.SetActive(weaponData != null);
            
            this.weaponData = weaponData;
            if (weaponData == null)
            {
                name.text = "";
                return;
            }

            name.text = this.weaponData.WeaponSpecVO.Name;
            prevResourceValue = -1;
            prevReloadValueIsZero = false;
        }

        public void OnUpdate()
        {
            if (weaponData == null)
            {
                return;
            }

            UpdateIconColor();
            UpdateTotalCollideCount();
            UpdateResourceValue();
            UpdateReloadTime();
        }

        void UpdateIconColor()
        {
            // SingleIconとFrontIcon
            if (weaponData.WeaponStateData.IsExecutable && weaponData.WeaponStateData.IsExecute)
            {
                singleWeaponIcon.color = executingColor;
            }
            else if (weaponData.WeaponStateData.IsExecutable)
            {
                singleWeaponIcon.color = executableColor;
            }
            else if (weaponData.WeaponStateData.ReloadRemainTime != 0)
            {
                singleWeaponIcon.color = reloadingColor;
            }
            else if (weaponData.WeaponSpecVO.MagazineSize == weaponData.WeaponStateData.ResourceIndex)
            {
                singleWeaponIcon.color = resourceEmptyColor;
            }
            else
            {
                singleWeaponIcon.color = disableColor;
            }
        }

        void UpdateTotalCollideCount()
        {
            var totalCollideCount = weaponData.WeaponStateData.CollideCount;
            if (prevTotalCollideCount != totalCollideCount)
            {
                prevTotalCollideCount = totalCollideCount;
                hitAnimator.SetTrigger(AnimatorKey.Play);
            }
        }

        void UpdateResourceValue()
        {
            if (prevResourceValue != weaponData.WeaponStateData.ResourceIndex)
            {
                // リソース表示
                prevResourceValue = weaponData.WeaponStateData.ResourceIndex;
                resourceCount.text = (weaponData.WeaponSpecVO.MagazineSize - prevResourceValue).ToString();
                resourceCount.color = weaponData.WeaponSpecVO.MagazineSize < prevResourceValue ? Color.red : Color.white;
            }
        }

        void UpdateReloadTime()
        {
            if (!prevReloadValueIsZero || weaponData.WeaponStateData.ReloadRemainTime != 0)
            {
                // リロード表示
                // 最後の1回が更新されないのでフラグでなんとかする
                prevReloadValueIsZero = weaponData.WeaponStateData.ReloadRemainTime == 0;
                var reloadRatio = 1.0f - (weaponData.WeaponStateData.ReloadRemainTime / weaponData.WeaponSpecVO.ReloadTime);

                var singleScale = singleReloadGaugeRect.localScale;
                singleScale.x = reloadRatio;
                singleReloadGaugeRect.localScale = singleScale;
            }
        }
    }
}
