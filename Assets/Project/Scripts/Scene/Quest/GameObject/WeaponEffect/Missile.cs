using UnityEngine;

namespace AloneSpace
{
    public class Missile : WeaponEffect
    {
        [SerializeField] TrailRenderer trailRenderer;

        MissileWeaponEffectData missileData;

        public override WeaponEffectData WeaponEffectData => missileData;

        public override void SetWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            missileData = (MissileWeaponEffectData) weaponEffectData;

            transform.position = missileData.Position;
            transform.rotation = missileData.Rotation;

            trailRenderer.Clear();
        }

        public override void OnLateUpdate()
        {
            transform.position = missileData.Position;
            transform.rotation = missileData.Rotation;
        }

        protected override void OnRelease()
        {
        }
    }
}