using System;
using UnityEngine;

namespace RoboQuest.Quest.InSide
{
    /// <summary>
    /// Pistol, Rifle, MachineGun, ShotGun, Rocket, Missile などの武器で生成される
    /// 寿命を持った飛翔体の基底クラス
    /// </summary>
    public abstract class Projectile : CacheableGameObject, IThreat, ICollision, ICauseDamage
    {
        public Guid PlayerInstanceId => WeaponData.PlayerInstanceId;

        public WeaponData WeaponData { get; protected set; }
        public ItemVO ResourceItemVO { get; protected set; }
        public ITarget Target { get; protected set; }

        public bool IsCollidable => IsActive;
        
        public abstract CollisionShape CollisionShape { get; protected set; }
        public abstract CollisionShape HitCollidePrediction { get; protected set; }

        protected float LifeTime { get; set; }
        protected float CurrentLifeTime { get; set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="resourceItemVO">リソースデータ</param>
        /// <param name="target">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低)</param>
        public virtual void Implement(WeaponData weaponData, ItemVO resourceItemVO, ITarget target, float condition)
        {
            WeaponData = weaponData;
            ResourceItemVO = resourceItemVO;
            Target = target;
            
            CurrentLifeTime = 0;
            IsActive = true;
        }

        protected abstract void OnUpdate(float deltaTime);

        void Update()
        {
            if (!IsActive)
            {
                return;
            }

            var deltaTime = Time.deltaTime;
            OnUpdate(deltaTime);
            
            CurrentLifeTime += deltaTime;
            if (CurrentLifeTime > LifeTime)
            {
                Release();
            }
        }
    }
}
