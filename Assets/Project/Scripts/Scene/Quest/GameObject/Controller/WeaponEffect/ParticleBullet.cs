using UnityEngine;

namespace AloneSpace
{
    public class ParticleBullet : WeaponEffect
    {
        [SerializeField] ParticleSystem particleSystem;

        ParticleBulletWeaponEffectData bulletData;

        public override WeaponEffectData WeaponEffectData => bulletData;

        protected override void OnInit(WeaponEffectData weaponEffectData)
        {
            bulletData = (ParticleBulletWeaponEffectData) weaponEffectData;

            // structだけどsetterで仲介しているのでこれで設定出来る
            var main = particleSystem.main;
            main.startSpeed = new ParticleSystem.MinMaxCurve(bulletData.SpecVO.Speed);
            var emission = particleSystem.emission;
            emission.rateOverTime = new ParticleSystem.MinMaxCurve(1.0f / bulletData.OptionData.ParticleBulletMakerWeaponData.VO.FireRate);
            var shape = particleSystem.shape;
            shape.angle = 1.0f / bulletData.OptionData.ParticleBulletMakerWeaponData.VO.Accuracy * 90.0f;

            transform.position = bulletData.Position;
            transform.rotation = bulletData.Rotation;

            particleSystem.Play();
            particleSystem.Stop();
        }

        public override void OnLateUpdate()
        {
            if (bulletData.OptionData.ParticleBulletMakerWeaponData.WeaponStateData.IsExecute)
            {
                if (!particleSystem.isEmitting)
                {
                    particleSystem.Play();
                }
            }
            else
            {
                if (particleSystem.isEmitting)
                {
                    particleSystem.Stop();
                }
            }

            transform.position = bulletData.Position;
            transform.rotation = bulletData.Rotation;
        }
    }
}
