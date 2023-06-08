namespace AloneSpace
{
    public class Explosion : WeaponEffect
    {
        ExplosionWeaponEffectData explosionData;

        public override WeaponEffectData WeaponEffectData => explosionData;

        protected override void OnInit(WeaponEffectData weaponEffectData)
        {
            explosionData = (ExplosionWeaponEffectData) weaponEffectData;

            transform.position = explosionData.Position;
            transform.rotation = explosionData.Rotation;
        }

        public override void OnLateUpdate()
        {
            transform.position = explosionData.Position;
            transform.rotation = explosionData.Rotation;
        }

        protected override void OnRelease()
        {
        }
    }
}