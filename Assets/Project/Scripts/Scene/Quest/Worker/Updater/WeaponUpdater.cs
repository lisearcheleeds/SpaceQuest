using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class WeaponUpdater : IUpdater
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

        public void OnLateUpdate()
        {
            if (questData == null)
            {
                return;
            }

            var deltaTime = Time.deltaTime;

            foreach (var actorData in questData.ActorData.Values)
            {
                // FIXME: 他色々成約付けて処理軽くする
                if (actorData.ActorState != ActorState.Alive)
                {
                    continue;
                }

                foreach (var weaponData in actorData.WeaponData)
                {
                    weaponData.OnLateUpdate(deltaTime);
                }
            }
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低)</param>
        void ExecuteTriggerWeapon(WeaponData weaponData, ITargetData targetData, float condition)
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
