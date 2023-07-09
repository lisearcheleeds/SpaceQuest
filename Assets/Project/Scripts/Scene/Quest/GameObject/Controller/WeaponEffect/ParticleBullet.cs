using UnityEngine;

namespace AloneSpace
{
    public class ParticleBullet : WeaponEffect
    {
        ParticleBulletWeaponEffectData bulletData;

        public override WeaponEffectData WeaponEffectData => bulletData;

        protected override void OnInit(WeaponEffectData weaponEffectData)
        {
            bulletData = (ParticleBulletWeaponEffectData) weaponEffectData;

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
