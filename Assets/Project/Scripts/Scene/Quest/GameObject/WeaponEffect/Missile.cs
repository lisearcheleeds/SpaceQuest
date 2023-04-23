namespace AloneSpace
{
    public class Missile : WeaponEffect
    {
        MissileWeaponEffectData missileData;

        public override WeaponEffectData WeaponEffectData => missileData;
        
        public override void SetWeaponEffectData(WeaponEffectData weaponEffectData)
        {
            missileData = (MissileWeaponEffectData) weaponEffectData;
            
            transform.position = missileData.Position;
            transform.rotation = missileData.Rotation;
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