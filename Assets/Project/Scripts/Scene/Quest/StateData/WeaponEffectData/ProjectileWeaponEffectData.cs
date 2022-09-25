using System;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    /// <summary>
    /// Pistol, Rifle, MachineGun, ShotGun, Rocket, Missile などの武器で生成される
    /// 寿命を持った飛翔体の基底クラス
    /// </summary>
    public abstract class ProjectileWeaponEffectData : WeaponEffectData
    {
        public override int AreaIndex { get; protected set; }
        public override Vector3 Position { get; protected set; }
        
        public override bool IsAlive { get; protected set; }
        public override Vector3 MoveDelta { get; protected set; }
        
        public override bool IsCollidable { get; protected set; }
        
        public override ITargetData TargetData { get; protected set; }

        protected float LifeTime { get; set; }
        protected float CurrentLifeTime { get; set; }

        /// <summary>
        /// 武器の使用
        /// </summary>
        /// <param name="weaponData">武器データ</param>
        /// <param name="targetData">ターゲット</param>
        /// <param name="condition">使用時の状態(1.0最高 ~ 0.0最低)</param>
        public ProjectileWeaponEffectData(WeaponData weaponData, ITargetData targetData, float condition) : base(weaponData)
        {
            AreaIndex = weaponData.BasePosition.AreaIndex;
            Position = weaponData.BasePosition.Position;
            
            IsAlive = true;
            IsCollidable = true;
            
            TargetData = targetData;
            
            CurrentLifeTime = 0;
        }

        public override void OnLateUpdate(float delta)
        {
            if (!IsAlive)
            {
                MessageBus.Instance.RemoveWeaponEffectData.Broadcast(this);
                return;
            }

            OnUpdate(delta);
            
            CurrentLifeTime += delta;
            if (CurrentLifeTime > LifeTime)
            {
                IsAlive = false;
            }
        }

        protected abstract void OnUpdate(float deltaTime);
    }
}
