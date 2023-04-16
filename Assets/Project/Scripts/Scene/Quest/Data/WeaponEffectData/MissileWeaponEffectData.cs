using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectData : WeaponEffectData
    {
        float lifeTime;
        float currentLifeTime;

        Vector3 direction;
        float speed;
        float rotateRatio;
        
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; protected set; }
        public override CollisionData CollisionData { get; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="fromPositionData">発射位置</param>
        /// <param name="rotation">方向</param>
        /// <param name="targetData">ターゲット</param>
        public MissileWeaponEffectData(WeaponData weaponData, IPositionData fromPositionData, Quaternion rotation, IPositionData targetData) : base(weaponData)
        {
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;

            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));
            
            TargetData = targetData;
            
            speed = 25.0f;
            lifeTime = 8;

            direction = rotation * Vector3.forward;
            rotateRatio = 0f;
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

            var targetDiffPosition = TargetData.Position - Position;
            direction = Vector3.RotateTowards(direction, targetDiffPosition, rotateRatio, 0);

            Position += direction * speed * deltaTime;
            
            if (currentLifeTime >= 1.8f)
            {
                rotateRatio = 0.1f;
            }

            if (currentLifeTime >= 2.0f)
            {
                rotateRatio = 0.002f;
            }
        }
    }
}
