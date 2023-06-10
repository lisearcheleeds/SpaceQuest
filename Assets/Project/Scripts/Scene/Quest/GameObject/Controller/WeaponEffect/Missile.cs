using UnityEngine;

namespace AloneSpace
{
    public class Missile : WeaponEffect
    {
        public override WeaponEffectData WeaponEffectData => missileData;

        [SerializeField] Transform smokePos;

        MissileWeaponEffectData missileData;
        MissileGraphicEffectHandler missileGraphicEffectHandler;

        protected override void OnInit(WeaponEffectData weaponEffectData)
        {
            missileData = (MissileWeaponEffectData) weaponEffectData;

            transform.position = missileData.Position;
            transform.rotation = missileData.Rotation;

            var weaponSpecVO = (WeaponMissileMakerSpecVO)missileData.WeaponData.WeaponSpecVO;
            missileGraphicEffectHandler = new MissileGraphicEffectHandler(new TransformPositionData(weaponEffectData, smokePos));
            MessageBus.Instance.SpawnGraphicEffect.Broadcast(weaponSpecVO.SmokeGraphicEffectSpecVO, missileGraphicEffectHandler);
        }

        public override void OnLateUpdate()
        {
            transform.position = missileData.Position;
            transform.rotation = missileData.Rotation;
        }

        protected override void OnRelease()
        {
            base.OnRelease();
            missileGraphicEffectHandler.Abandon();
        }
    }
}