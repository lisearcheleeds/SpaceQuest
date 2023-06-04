using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponEffectObjectUpdater
    {
        QuestData questData;

        Transform variableParent;

        AreaData observeAreaData;
        bool isDirty;

        List<WeaponEffect> weaponEffectList = new List<WeaponEffect>();

        public void Initialize(QuestData questData, Transform variableParent)
        {
            this.questData = questData;
            this.variableParent = variableParent;

            MessageBus.Instance.SetDirtyWeaponEffectObjectList.AddListener(SetDirtyWeaponEffectObjectList);
            MessageBus.Instance.AddWeaponEffectData.AddListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.AddListener(RemoveWeaponEffectData);
            MessageBus.Instance.SetUserArea.AddListener(SetUserArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyWeaponEffectObjectList.RemoveListener(SetDirtyWeaponEffectObjectList);
            MessageBus.Instance.AddWeaponEffectData.RemoveListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.RemoveListener(RemoveWeaponEffectData);
            MessageBus.Instance.SetUserArea.RemoveListener(SetUserArea);
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }

            foreach (var weaponEffect in weaponEffectList)
            {
                weaponEffect.OnLateUpdate();
            }
        }

        void SetUserArea(AreaData areaData)
        {
            this.observeAreaData = areaData;
            SetDirtyWeaponEffectObjectList();
        }

        /// <summary>
        /// 重いので基本Areaの切り替わりなどでのみ使用する
        /// </summary>
        void Refresh()
        {
            foreach (var weaponEffect in weaponEffectList.ToArray())
            {
                // 違うエリアだったり、questData.WeaponEffectDataに存在しないweaponEffectであれば削除
                if (!questData.WeaponEffectData.ContainsKey(weaponEffect.WeaponEventEffectData.InstanceId) || weaponEffect.WeaponEventEffectData.AreaId != observeAreaData?.AreaId)
                {
                    ReleaseWeaponEffect(weaponEffect);
                }
            }

            foreach (var weaponEffectData in questData.WeaponEffectData.Values)
            {
                // 同じエリアでweaponEffectListに存在しないweaponEffectDataであれば生成
                if (weaponEffectData.AreaId == observeAreaData?.AreaId && weaponEffectList.All(weaponEffect => weaponEffect.WeaponEventEffectData.InstanceId != weaponEffectData.InstanceId))
                {
                    CreateWeaponEffect(weaponEffectData);
                }
            }
        }

        void CreateWeaponEffect(WeaponEventEffectData weaponEventEffectData)
        {
            var assetPathVO = weaponEventEffectData.WeaponData.WeaponSpecVO switch
            {
                WeaponBulletMakerSpecVO bullet => bullet.ProjectilePath,
                WeaponMissileMakerSpecVO missile => missile.ProjectilePath,
                _ => throw new NotImplementedException(),
            };

            GameObjectCache.Instance.GetAsset<WeaponEffect>(
                assetPathVO,
                weaponEffect =>
                {
                    weaponEffect.IsActive = true;
                    weaponEffect.transform.SetParent(variableParent, false);
                    weaponEffect.Init(weaponEventEffectData);
                    weaponEffectList.Add(weaponEffect);
                });
        }

        void ReleaseWeaponEffect(WeaponEffect weaponEffect)
        {
            weaponEffect.Release();
            weaponEffectList.Remove(weaponEffect);
        }

        void SetDirtyWeaponEffectObjectList()
        {
            isDirty = true;
        }

        void AddWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            if (weaponEventEffectData.AreaId == observeAreaData?.AreaId)
            {
                CreateWeaponEffect(weaponEventEffectData);
            }
        }

        void RemoveWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            if (weaponEventEffectData.AreaId == observeAreaData?.AreaId)
            {
                ReleaseWeaponEffect(weaponEffectList.First(x => x.WeaponEventEffectData.InstanceId == weaponEventEffectData.InstanceId));
            }
        }
    }
}