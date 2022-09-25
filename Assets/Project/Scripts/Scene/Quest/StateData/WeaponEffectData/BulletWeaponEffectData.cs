using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectData : ProjectileWeaponEffectData
    {
        Vector3 direction;
        float speed;
        
        public override CollisionShape CollisionShape { get; protected set; }
        public override CollisionShape HitCollidePrediction { get; protected set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低) Bulletではdirectionに影響</param>
        public BulletWeaponEffectData(WeaponData weaponData, ITargetData targetData, float condition) : base(weaponData, targetData, condition)
        {
            speed = 200.0f;
            LifeTime = 4;

            direction = weaponData.OffsetRotation * Vector3.forward;

            CollisionShape = new CollisionShapeSphere(this, 1.0f);
            HitCollidePrediction = new CollisionShapeLine(this, direction, 1.0f);
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
            Position += direction * speed * deltaTime;
        }
    }
}
