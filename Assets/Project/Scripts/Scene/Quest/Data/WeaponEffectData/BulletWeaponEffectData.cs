using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectData : WeaponEffectData
    {
        float lifeTime;
        float currentLifeTime;
        
        Vector3 direction;
        float speed;
        
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; protected set; }
        public override CollisionData CollisionData { get; }
        
        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public BulletWeaponEffectData(WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData) : base(weaponData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            CollisionEffectSenderModule = new CollisionEffectSenderModule();
            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));
            
            speed = 200.0f;
            lifeTime = 4;

            direction = rotation * Vector3.forward;
            currentLifeTime = 0;
                
            ActivateModules();
        }
        
        public override void ActivateModules()
        {
            base.ActivateModules();
            
            CollisionEffectSenderModule = new CollisionEffectSenderModule();
            CollisionEffectSenderModule.ActivateModule();
        }

        public virtual void DeactivateModules()
        {
            base.ActivateModules();
            
            CollisionEffectSenderModule.DeactivateModule();
            CollisionEffectSenderModule = null;
        }

        protected override void OnBeginModuleUpdate(float deltaTime)
        {
            if (!IsAlive)
            {
                MessageBus.Instance.RemoveWeaponEffectData.Broadcast(this);
                return;
            }
            
            currentLifeTime += deltaTime;
            if (currentLifeTime > lifeTime)
            {
                IsAlive = false;
            }

            if (!IsAlive)
            {
                return;
            }

            if (CollisionData.IsCollided)
            {
                IsAlive = false;
                return;
            }

            Position += direction * speed * deltaTime;
        }
    }
}
