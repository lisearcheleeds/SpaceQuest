using System;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.CreateWeaponEffectData.AddListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.AddListener(ReleaseWeaponEffectData);

            MessageBus.Instance.AddWeaponEffectData.AddListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.AddListener(RemoveWeaponEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);

            MessageBus.Instance.AddWeaponEffectData.RemoveListener(AddWeaponEffectData);
            MessageBus.Instance.RemoveWeaponEffectData.RemoveListener(RemoveWeaponEffectData);
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        void CreateWeaponEffectData(WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData)
        {
            WeaponEventEffectData weaponEventEffectData = weaponData.WeaponSpecVO switch
            {
                WeaponBulletMakerSpecVO parameterVO => new BulletWeaponEventEffectData(weaponData, parameterVO, fromPositionData, rotation, targetData),
                WeaponMissileMakerSpecVO parameterVO => new MissileWeaponEventEffectData(weaponData, parameterVO, fromPositionData, rotation, targetData),
                _ => throw new NotImplementedException(),
            };

            questData.WeaponEffectData.Add(weaponEventEffectData.InstanceId, weaponEventEffectData);

            MessageBus.Instance.AddWeaponEffectData.Broadcast(weaponEventEffectData);
        }

        void ReleaseWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            questData.WeaponEffectData.Remove(weaponEventEffectData.InstanceId);

            MessageBus.Instance.RemoveWeaponEffectData.Broadcast(weaponEventEffectData);
        }

        void AddWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            questData.ActorData[weaponEventEffectData.WeaponData.WeaponHolder.InstanceId].AddWeaponEffectData(weaponEventEffectData);
        }

        void RemoveWeaponEffectData(WeaponEventEffectData weaponEventEffectData)
        {
            questData.ActorData[weaponEventEffectData.WeaponData.WeaponHolder.InstanceId].RemoveWeaponEffectData(weaponEventEffectData);
        }
    }
}