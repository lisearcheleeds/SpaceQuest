using UnityEngine;

namespace AloneSpace
{
    public class Bullet : WeaponEffect
    {
        BulletWeaponEventEffectData bulletData;

        public override WeaponEventEffectData WeaponEventEffectData => bulletData;

        protected override void OnInit(WeaponEventEffectData weaponEventEffectData)
        {
            bulletData = (BulletWeaponEventEffectData) weaponEventEffectData;

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