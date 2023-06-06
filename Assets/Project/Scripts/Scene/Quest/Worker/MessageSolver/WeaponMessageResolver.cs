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
        /// <param name="weaponEffectSpecVO">武器データ</param>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        void CreateWeaponEffectData(IWeaponEffectSpecVO weaponEffectSpecVO, WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData)
        {
            WeaponEffectData weaponEffectData = weaponEffectSpecVO switch
            {
                BulletWeaponEffectSpecVO specVO => new BulletWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                MissileWeaponEffectSpecVO specVO => new MissileWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                ExplosionWeaponEffectSpecVO specVO => new ExplosionWeaponEffectData(specVO, weaponData, fromPositionData, rotation, targetData),
                _ => throw new NotImplementedException(),
            };

            questData.WeaponEffectData.Add(weaponEffectData.InstanceId, weaponEffectData);

            MessageBus.Instance.AddWeaponEffectData.Broadcast(weaponEffectData);
        }

        void ReleaseWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.WeaponEffectData.Remove(weaponEffectData.InstanceId);

            MessageBus.Instance.RemoveWeaponEffectData.Broadcast(weaponEffectData);
        }

        void AddWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].AddWeaponEffectData(weaponEffectData);
        }

        void RemoveWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            questData.ActorData[weaponEffectData.WeaponData.WeaponHolder.InstanceId].RemoveWeaponEffectData(weaponEffectData);
        }
    }
}