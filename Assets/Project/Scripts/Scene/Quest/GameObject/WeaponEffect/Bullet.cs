using UnityEngine;

namespace AloneSpace
{
    public class Bullet : WeaponEffect
    {
        BulletWeaponEffectData bulletData;

        public override WeaponEffectData WeaponEffectData => bulletData;
        
        public override void SetWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            bulletData = (BulletWeaponEffectData) weaponEffectData;
            
            transform.position = bulletData.Position;
            transform.rotation = bulletData.Rotation;
        }

        public override void OnLateUpdate()
        {
            transform.position = bulletData.Position;
            transform.rotation = bulletData.Rotation;
        }

        protected override void OnRelease()
        {
        }
    }
}