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
        
        public override CollisionEffectSenderModule CollisionEffectSenderModule { get; }
        public override CollisionData CollisionData { get; }
        
        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低) Bulletではdirectionに影響</param>
        public BulletWeaponEffectData(WeaponData weaponData, IPositionData targetData, float condition) : base(weaponData)
        {
            AreaId = weaponData.PositionData.AreaId;
            Position = weaponData.PositionData.Position;
            Rotation = weaponData.PositionData.Rotation;

            CollisionEffectSenderModule = new CollisionEffectSenderModule();
            CollisionData = new CollisionData(this, new CollisionShapeSphere(this, 1.0f));
            
            speed = 200.0f;
            lifeTime = 4;

            direction = weaponData.OffsetRotation * Vector3.forward;
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

            Position += direction * speed * deltaTime;
        }
    }
}
