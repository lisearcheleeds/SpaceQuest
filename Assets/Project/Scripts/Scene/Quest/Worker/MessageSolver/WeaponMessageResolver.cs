using System;

namespace AloneSpace
{
    public class WeaponMessageResolver
    {
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
            
            MessageBus.Instance.ExecuteTriggerWeapon.AddListener(ExecuteTriggerWeapon);
        }

        public void Finalize()
        {
            MessageBus.Instance.ExecuteTriggerWeapon.RemoveListener(ExecuteTriggerWeapon);
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低)</param>
        void ExecuteTriggerWeapon(WeaponData weaponData, IPositionData targetData, float condition)
        {
            switch (weaponData.ActorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponRifleParameterVO rifleParameterVO:
                    MessageBus.Instance.AddWeaponEffectData.Broadcast(new BulletWeaponEffectData(weaponData, targetData, condition));
                    return;
                case ActorPartsWeaponMissileLauncherParameterVO missileLauncherParameterVO:
                    MessageBus.Instance.AddWeaponEffectData.Broadcast(new MissileWeaponEffectData(weaponData, targetData, condition));
                    return;
            }

            throw new NotImplementedException();
        }
    }
}