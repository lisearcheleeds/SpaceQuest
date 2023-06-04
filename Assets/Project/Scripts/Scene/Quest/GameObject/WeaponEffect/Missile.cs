using UnityEngine;

namespace AloneSpace
{
    public class Missile : WeaponEffect
    {
        [SerializeField] TrailRenderer trailRenderer;

        MissileWeaponEventEffectData missileData;

        public override WeaponEventEffectData WeaponEventEffectData => missileData;

        protected override void OnInit(WeaponEventEffectData weaponEventEffectData)
        {
            missileData = (MissileWeaponEventEffectData) weaponEventEffectData;

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