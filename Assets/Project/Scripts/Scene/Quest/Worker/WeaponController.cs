using System;
using System.Linq;
using RoboQuest.Quest.InSide;

namespace RoboQuest.Quest
{
    public class WeaponController
    {
        ITarget[] targets;
        
        public void Initialize()
        {
            MessageBus.Instance.ExecuteTriggerWeapon.AddListener(ExecuteTriggerWeapon);
            MessageBus.Instance.SubscribeUpdateTargetList.AddListener(SubscribeUpdateTargetList);
        }

        public void Finalize()
        {
            MessageBus.Instance.ExecuteTriggerWeapon.RemoveListener(ExecuteTriggerWeapon);
            MessageBus.Instance.SubscribeUpdateTargetList.RemoveListener(SubscribeUpdateTargetList);
        }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="itemVO">リソースデータ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低)</param>
        void ExecuteTriggerWeapon(WeaponData weaponData, ItemVO itemVO, ITargetData targetData, float condition)
        {
            var target = targets?.FirstOrDefault(x => x.TargetData.InstanceId == targetData.InstanceId);

            if (target == null)
            {
                if (targetData is IDamageableData damageableData)
                {
                    // とりあえず必中として処理
                    MessageBus.Instance.NoticeDamage.Broadcast(weaponData, itemVO, damageableData);
                }

                return;
            }
            
            switch (weaponData.ActorPartsWeaponParameterVO)
            {
                case ActorPartsWeaponRifleParameterVO rifleParameterVO:
                    GameObjectCache.Instance.GetAsset<Bullet>(
                        rifleParameterVO.ProjectilePath,
                        asset => asset.Implement(weaponData, itemVO, target, condition));
                    return;
                case ActorPartsWeaponMissileLauncherParameterVO missileLauncherParameterVO:
                    GameObjectCache.Instance.GetAsset<Missile>(
                        missileLauncherParameterVO.ProjectilePath,
                        asset => asset.Implement(weaponData, itemVO, target, condition));
                    return;
            }

            throw new NotImplementedException();
        }
        
        void SubscribeUpdateTargetList(ITarget[] targets)
        {
            this.targets = targets;
        }
    }
}
