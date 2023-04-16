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
            
            MessageBus.Instance.ExecuteWeapon.AddListener(ExecuteTriggerWeapon);
        }

        public void Finalize()
        {
            MessageBus.Instance.ExecuteWeapon.RemoveListener(ExecuteTriggerWeapon);
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        void ExecuteTriggerWeapon(WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData)
        {
            switch (weaponData.ActorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponBulletMakerParameterVO:
                    MessageBus.Instance.AddWeaponEffectData.Broadcast(new BulletWeaponEffectData(weaponData, fromPositionData, rotation, targetData));
                    return;
                case ActorPartsWeaponMissileMakerParameterVO:
                    MessageBus.Instance.AddWeaponEffectData.Broadcast(new MissileWeaponEffectData(weaponData, fromPositionData, rotation, targetData));
                    return;
            }

            throw new NotImplementedException();
        }
    }
}