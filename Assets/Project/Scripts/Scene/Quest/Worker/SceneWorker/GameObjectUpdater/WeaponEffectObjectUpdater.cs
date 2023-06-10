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
                if (!questData.WeaponEffectData.ContainsKey(weaponEffect.WeaponEffectData.InstanceId) || weaponEffect.WeaponEffectData.AreaId != observeAreaData?.AreaId)
                {
                    ReleaseWeaponEffect(weaponEffect);
                }
            }

            foreach (var weaponEffectData in questData.WeaponEffectData.Values)
            {
                // 同じエリアでweaponEffectListに存在しないweaponEffectDataであれば生成
                if (weaponEffectData.AreaId == observeAreaData?.AreaId && weaponEffectList.All(weaponEffect => weaponEffect.WeaponEffectData.InstanceId != weaponEffectData.InstanceId))
                {
                    CreateWeaponEffect(weaponEffectData);
                }
            }
        }

        void CreateWeaponEffect(WeaponEffectData weaponEffectData)
        {
            GameObjectCache.Instance.GetAsset<WeaponEffect>(
                weaponEffectData.WeaponEffectSpecVO.Path,
                weaponEffect =>
                {
                    weaponEffect.transform.SetParent(variableParent, false);
                    weaponEffect.Init(weaponEffectData);
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

        void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeAreaData?.AreaId)
            {
                CreateWeaponEffect(weaponEffectData);
            }
        }

        void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeAreaData?.AreaId)
            {
                ReleaseWeaponEffect(weaponEffectList.First(x => x.WeaponEffectData.InstanceId == weaponEffectData.InstanceId));
            }
        }
    }
}