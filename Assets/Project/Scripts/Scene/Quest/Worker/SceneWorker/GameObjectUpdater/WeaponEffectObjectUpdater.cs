using System;
using System.Linq;
using System.Collections.Generic;

namespace AloneSpace
{
    public class WeaponEffectObjectUpdater
    {
        QuestData questData;

        AreaData observeArea;
        bool isDirty;

        List<WeaponEffect> currentWeaponEffectList = new List<WeaponEffect>();
        List<Guid> loadingWeaponEffect = new List<Guid>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            MessageBus.Instance.SetDirtyWeaponEffectObjectList.AddListener(SetDirtyWeaponEffectObjectList);
            MessageBus.Instance.CreatedWeaponEffectData.AddListener(AddWeaponEffectData);
            MessageBus.Instance.ReleasedWeaponEffectData.AddListener(RemoveWeaponEffectData);
            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyWeaponEffectObjectList.RemoveListener(SetDirtyWeaponEffectObjectList);
            MessageBus.Instance.CreatedWeaponEffectData.RemoveListener(AddWeaponEffectData);
            MessageBus.Instance.ReleasedWeaponEffectData.RemoveListener(RemoveWeaponEffectData);
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
                Refresh();
            }

            foreach (var currentWeaponEffect in currentWeaponEffectList)
            {
                currentWeaponEffect.OnLateUpdate();
            }
        }

        void SetUserObserveArea(AreaData areaData)
        {
            this.observeArea = areaData;
            SetDirtyWeaponEffectObjectList();
        }

        /// <summary>
        /// 重いので基本Areaの切り替わりなどでのみ使用する
        /// </summary>
        void Refresh()
        {
            WeaponEffectData[] shouldWeaponEffectDataList;

            // TODO: エリア外をAreaIdの組み合わせで定義する
            if (observeArea != null)
            {
                // 現在のエリア内のすべてのWeaponEffect
                shouldWeaponEffectDataList = questData.WeaponEffectData.Values.Where(weaponEffectData => weaponEffectData.AreaId == observeArea.AreaId).ToArray();
            }
            else
            {
                shouldWeaponEffectDataList = new WeaponEffectData[0];
            }

            foreach (var currentWeaponEffect in currentWeaponEffectList.ToArray())
            {
                // 違うエリアだったり、questData.WeaponEffectDataに存在しないweaponEffectであれば削除
                if (shouldWeaponEffectDataList.All(x => x.InstanceId != currentWeaponEffect.WeaponEffectData.InstanceId))
                {
                    ReleaseWeaponEffect(currentWeaponEffect);
                }
            }

            foreach (var shouldWeaponEffectData in shouldWeaponEffectDataList)
            {
                // 同じエリアでweaponEffectListに存在しないweaponEffectDataであれば生成
                if (currentWeaponEffectList.All(weaponEffect => weaponEffect.WeaponEffectData.InstanceId != shouldWeaponEffectData.InstanceId))
                {
                    if (!loadingWeaponEffect.Contains(shouldWeaponEffectData.InstanceId))
                    {
                        CreateWeaponEffect(shouldWeaponEffectData);
                    }
                }
            }
        }

        void CreateWeaponEffect(WeaponEffectData weaponEffectData)
        {
            loadingWeaponEffect.Add(weaponEffectData.InstanceId);
            MessageBus.Instance.GetCacheAsset.Broadcast(weaponEffectData.WeaponEffectSpecVO.Path, c =>
            {
                var weaponEffect = (WeaponEffect)c;
                weaponEffect.Init(weaponEffectData);
                currentWeaponEffectList.Add(weaponEffect);
                loadingWeaponEffect.Remove(weaponEffectData.InstanceId);
            });
        }

        void ReleaseWeaponEffect(WeaponEffect weaponEffect)
        {
            weaponEffect.Release();
            currentWeaponEffectList.Remove(weaponEffect);
        }

        void SetDirtyWeaponEffectObjectList()
        {
            isDirty = true;
        }

        void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeArea?.AreaId)
            {
                CreateWeaponEffect(weaponEffectData);
            }
        }

        void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeArea?.AreaId)
            {
                ReleaseWeaponEffect(currentWeaponEffectList.First(x => x.WeaponEffectData.InstanceId == weaponEffectData.InstanceId));
            }
        }
    }
}
