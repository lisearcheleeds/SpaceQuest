using UnityEngine;

namespace AloneSpace
{
    public class Bullet : WeaponEffect
    {
        BulletWeaponEffectData bulletData;

        public override WeaponEffectData WeaponEffectData => bulletData;

        protected override void OnInit(WeaponEffectData weaponEffectData)
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
    }
}