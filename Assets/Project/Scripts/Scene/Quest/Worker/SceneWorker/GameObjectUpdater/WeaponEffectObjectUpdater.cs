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

        LinkedList<WeaponEffect> currentWeaponEffectList = new LinkedList<WeaponEffect>();
        LinkedList<Guid> loadingWeaponEffect = new LinkedList<Guid>();

        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            MessageBus.Instance.Creator.OnCreateWeaponEffectData.AddListener(OnCreateWeaponEffectData);
            MessageBus.Instance.Creator.OnReleaseWeaponEffectData.AddListener(OnReleaseWeaponEffectData);
            MessageBus.Instance.Temp.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.Creator.OnCreateWeaponEffectData.RemoveListener(OnCreateWeaponEffectData);
            MessageBus.Instance.Creator.OnReleaseWeaponEffectData.RemoveListener(OnReleaseWeaponEffectData);
            MessageBus.Instance.Temp.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void OnUpdate()
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
            isDirty = true;
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
            loadingWeaponEffect.AddLast(weaponEffectData.InstanceId);
            MessageBus.Instance.GetCacheAsset.Broadcast(weaponEffectData.WeaponEffectSpecVO.Path, c =>
            {
                var weaponEffect = (WeaponEffect)c;
                weaponEffect.Init(weaponEffectData);
                currentWeaponEffectList.AddLast(weaponEffect);
                loadingWeaponEffect.Remove(weaponEffectData.InstanceId);
            });
        }

        void ReleaseWeaponEffect(WeaponEffect weaponEffect)
        {
            weaponEffect.Release();
            currentWeaponEffectList.Remove(weaponEffect);
        }

        void OnCreateWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeArea?.AreaId)
            {
                CreateWeaponEffect(weaponEffectData);
            }
        }

        void OnReleaseWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            if (weaponEffectData.AreaId == observeArea?.AreaId)
            {
                ReleaseWeaponEffect(currentWeaponEffectList.First(x => x.WeaponEffectData.InstanceId == weaponEffectData.InstanceId));
            }
        }
    }
}
