using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectData : ProjectileWeaponEffectData
    {
        Vector3 direction;
        float speed;
        float rotateRatio;

        public override CollisionShape CollisionShape { get; protected set; }
        public override CollisionShape HitCollidePrediction { get; protected set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低) missileでは使用しない</param>
        public MissileWeaponEffectData(WeaponData weaponData, ITargetData targetData, float condition) : base(weaponData, targetData, condition)
        {
            speed = 25.0f;
            LifeTime = 8;

            direction = weaponData.OffsetRotation * Vector3.forward;
            rotateRatio = 0f;
            
            CollisionShape = new CollisionShapeSphere(this, 1.0f);
            HitCollidePrediction = new CollisionShapeCone(this, direction, 0.5f);
        }

        public override void OnCollision(ICollisionData otherCollisionData)
        {
            if (otherCollisionData.PlayerInstanceId != PlayerInstanceId)
            {
                IsAlive = false;
            }
        }

        protected override void OnUpdate(float deltaTime)
        {
            if (TargetData.IsAlive)
            {
                var targetDiffPosition = TargetData.Position - Position;
                direction = Vector3.RotateTowards(direction, targetDiffPosition, rotateRatio, 0);
            }

            Position += direction * speed * deltaTime;
            (HitCollidePrediction as CollisionShapeCone).Directon = direction;
            
            if (CurrentLifeTime >= 1.8f)
            {
                rotateRatio = 0.1f;
            }

            if (CurrentLifeTime >= 2.0f)
            {
                rotateRatio = 0.002f;
            }
        }
    }
}
