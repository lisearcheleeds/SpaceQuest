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
        }

        public void Finalize()
        {
            MessageBus.Instance.CreateWeaponEffectData.RemoveListener(CreateWeaponEffectData);
            MessageBus.Instance.ReleaseWeaponEffectData.RemoveListener(ReleaseWeaponEffectData);
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
            WeaponEffectData weaponEffectData = weaponData.ActorPartsWeaponParameterVO switch
            {
                ActorPartsWeaponBulletMakerParameterVO parameterVO => new BulletWeaponEffectData(weaponData, parameterVO, fromPositionData, rotation, targetData),
                ActorPartsWeaponMissileMakerParameterVO parameterVO => new MissileWeaponEffectData(weaponData, parameterVO, fromPositionData, rotation, targetData),
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
    }
}