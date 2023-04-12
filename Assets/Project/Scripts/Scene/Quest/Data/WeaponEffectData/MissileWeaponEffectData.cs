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
        
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; }
        public override CollisionData CollisionData { get; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低) missileでは使用しない</param>
        public MissileWeaponEffectData(WeaponData weaponData, IPositionData targetData, float condition) : base(weaponData)
        {
            AreaId = weaponData.PositionData.AreaId;
            Position = weaponData.PositionData.Position;
            Rotation = weaponData.PositionData.Rotation;

            CollisionEffectSenderModule = new CollisionEffectSenderModule();
            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));
            
            TargetData = targetData;
            
            speed = 25.0f;
            lifeTime = 8;

            direction = weaponData.OffsetRotation * Vector3.forward;
            rotateRatio = 0f;
            currentLifeTime = 0;
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
