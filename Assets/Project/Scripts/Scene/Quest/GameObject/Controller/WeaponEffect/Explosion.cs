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

            var weaponSpecVO = (IExplosionGraphicEffectSpecVOHolder)explosionData.WeaponData.WeaponSpecVO;
            MessageBus.Instance.SpawnGraphicEffect.Broadcast(weaponSpecVO.ExplosionGraphicEffectSpecVO, new ExplosionGraphicEffectHandler(explosionData));
        }

        public override void OnLateUpdate()
        {
            transform.position = explosionData.Position;
            transform.rotation = explosionData.Rotation;
        }
    }
}