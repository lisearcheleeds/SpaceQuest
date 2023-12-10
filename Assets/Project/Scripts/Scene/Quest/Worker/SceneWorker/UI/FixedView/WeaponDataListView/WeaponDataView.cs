using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class WeaponDataView : MonoBehaviour
    {
        [SerializeField] Animator weaponDataViewAnimator;
        [SerializeField] Animator hitAnimator;

        [SerializeField] Image singleWeaponIcon;
        [SerializeField] Image multipleWeaponIconFront;
        [SerializeField] Image multipleWeaponIconBack;
        [SerializeField] RectTransform singleReloadGaugeRect;
        [SerializeField] RectTransform multipleReloadGaugeRectFront;
        [SerializeField] RectTransform multipleReloadGaugeRectBack;
        [SerializeField] Text weaponName;
        [SerializeField] Text resourceCount;
        [SerializeField] Text weaponCount;

        [SerializeField] Color executableColor;
        [SerializeField] Color executingColor;
        [SerializeField] Color reloadingColor;
        [SerializeField] Color resourceEmptyColor;
        [SerializeField] Color disableColor;

        WeaponData[] weaponDataList;

        int prevTotalCollideCount;
        int prevResourceValue;
        bool prevReloadValueIsZero;

        bool isInit;

        public void SetWeaponDataList(WeaponData[] weaponDataList)
        {
            Init();

            // 何も変化がなければアニメーションはskip
            var prevWeaponIds = this.weaponDataList?.Select(weaponData => weaponData.InstanceId);
            var nextWeaponIds = weaponDataList?.Select(weaponData => weaponData.InstanceId);
            var skipSwitchAnim = (weaponDataList == null && this.weaponDataList == null) || (prevWeaponIds != null && nextWeaponIds != null && prevWeaponIds.SequenceEqual(nextWeaponIds));
            if (!skipSwitchAnim)
            {
                if ((this.weaponDataList?.Length ?? 0) == 0)
                {
                    // 無し→有りの状態変化であればIn
                    weaponDataViewAnimator.SetTrigger(AnimatorKey.In);
                }
                else
                {
                    // 有り→無しの状態変化であればOut
                    // 有り→有りの状態変化であればOut後にIn
                    weaponDataViewAnimator.SetTrigger(AnimatorKey.Out);
                    weaponDataViewAnimator.SetBool(AnimatorKey.Use, (weaponDataList?.Length ?? 0) != 0);
                }
            }

            this.weaponDataList = weaponDataList;
            if (weaponDataList == null)
            {
                return;
            }

            singleWeaponIcon.gameObject.SetActive(weaponDataList.Length == 1);
            multipleWeaponIconFront.gameObject.SetActive(weaponDataList.Length != 1);
            multipleWeaponIconBack.gameObject.SetActive(weaponDataList.Length != 1);
            singleReloadGaugeRect.gameObject.SetActive(weaponDataList.Length == 1);
            multipleReloadGaugeRectFront.gameObject.SetActive(weaponDataList.Length != 1);
            multipleReloadGaugeRectBack.gameObject.SetActive(weaponDataList.Length != 1);
            weaponCount.text = weaponDataList.Length == 1 ? "" : $"x{weaponDataList.Length}";

            prevResourceValue = -1;
            prevReloadValueIsZero = false;
        }

        public void OnUpdate()
        {
            if (weaponDataList == null)
            {
                return;
            }

            UpdateIconColor();
            UpdateTotalCollideCount();
            UpdateResourceValue();
            UpdateReloadTime();
        }

        void Awake()
        {
            Init();
        }

        void Init()
        {
            if (isInit)
            {
                return;
            }

            // 初回はとりあえず隠す
            weaponDataViewAnimator.SetTrigger(AnimatorKey.Out);
            weaponDataViewAnimator.SetBool(AnimatorKey.Use, false);
            isInit = true;
        }

        void UpdateIconColor()
        {
            // SingleIconとFrontIcon
            if (weaponDataList.Any(weaponData => weaponData.WeaponStateData.IsExecutable && weaponData.WeaponStateData.IsExecute))
            {
                singleWeaponIcon.color = executingColor;
                multipleWeaponIconFront.color = executingColor;
            }
            else if (weaponDataList.Any(weaponData => weaponData.WeaponStateData.IsExecutable))
            {
                singleWeaponIcon.color = executableColor;
                multipleWeaponIconFront.color = executableColor;
            }
            else if (weaponDataList.Any(weaponData => weaponData.WeaponStateData.ReloadRemainTime != 0))
            {
                singleWeaponIcon.color = reloadingColor;
                multipleWeaponIconFront.color = reloadingColor;
            }
            else if (weaponDataList.Any(weaponData => weaponData.WeaponSpecVO.MagazineSize == weaponData.WeaponStateData.ResourceIndex))
            {
                singleWeaponIcon.color = resourceEmptyColor;
                multipleWeaponIconFront.color = reloadingColor;
            }
            else
            {
                singleWeaponIcon.color = disableColor;
                multipleWeaponIconFront.color = disableColor;
            }

            // BackIcon
            if (weaponDataList.All(weaponData => weaponData.WeaponStateData.IsExecutable && weaponData.WeaponStateData.IsExecute))
            {
                multipleWeaponIconBack.color = executingColor;
            }
            else if (weaponDataList.All(weaponData => weaponData.WeaponStateData.IsExecutable))
            {
                multipleWeaponIconBack.color = executableColor;
            }
            else if (weaponDataList.All(weaponData => weaponData.WeaponStateData.ReloadRemainTime != 0))
            {
                multipleWeaponIconBack.color = reloadingColor;
            }
            else if (weaponDataList.All(weaponData => weaponData.WeaponSpecVO.MagazineSize == weaponData.WeaponStateData.ResourceIndex))
            {
                multipleWeaponIconBack.color = reloadingColor;
            }
            else
            {
                multipleWeaponIconBack.color = disableColor;
            }
        }

        void UpdateTotalCollideCount()
        {
            var totalCollideCount = weaponDataList.Sum(weaponData => weaponData.WeaponStateData.CollideCount);
            if (prevTotalCollideCount != totalCollideCount)
            {
                prevTotalCollideCount = totalCollideCount;
                hitAnimator.SetTrigger(AnimatorKey.Play);
            }
        }

        void UpdateResourceValue()
        {
            if (prevResourceValue != weaponDataList.Max(weaponData => weaponData.WeaponStateData.ResourceIndex))
            {
                // リソース表示
                prevResourceValue = weaponDataList.Max(weaponData => weaponData.WeaponStateData.ResourceIndex);
                var minMagazine = weaponDataList.Min(weaponData => weaponData.WeaponSpecVO.MagazineSize);
                var maxMagazine = weaponDataList.Max(weaponData => weaponData.WeaponSpecVO.MagazineSize);
                resourceCount.text = (maxMagazine - prevResourceValue).ToString();
                resourceCount.color = minMagazine < prevResourceValue ? Color.red : Color.white;
            }
        }

        void UpdateReloadTime()
        {
            if (!prevReloadValueIsZero || weaponDataList.Any(weaponData => weaponData.WeaponStateData.ReloadRemainTime != 0))
            {
                // リロード表示
                // 最後の1回が更新されないのでフラグでなんとかする
                prevReloadValueIsZero = weaponDataList.Any(weaponData => weaponData.WeaponStateData.ReloadRemainTime == 0);
                var maxReloadRatio = weaponDataList.Max(weaponData => 1.0f - (weaponData.WeaponStateData.ReloadRemainTime / weaponData.WeaponSpecVO.ReloadTime));
                var minReloadRatio = weaponDataList.Min(weaponData => 1.0f - (weaponData.WeaponStateData.ReloadRemainTime / weaponData.WeaponSpecVO.ReloadTime));

                var singleScale = singleReloadGaugeRect.localScale;
                singleScale.x = maxReloadRatio;
                singleReloadGaugeRect.localScale = singleScale;

                var multipleFrontScale = multipleReloadGaugeRectFront.localScale;
                multipleFrontScale.x = maxReloadRatio;
                multipleReloadGaugeRectFront.localScale = multipleFrontScale;

                var multipleBackScale = multipleReloadGaugeRectBack.localScale;
                multipleBackScale.x = minReloadRatio;
                multipleReloadGaugeRectBack.localScale = multipleBackScale;
            }
        }
    }
}
