using System;
using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData => ParticleBulletMakerWeaponData;

        public ParticleBulletMakerWeaponData ParticleBulletMakerWeaponData { get; }
        public Func<IPositionData> GetFromPositionData { get; }
        public Func<Quaternion> GetOffsetRotation { get; }
        public IPositionData TargetData { get; }

        public ParticleBulletWeaponEffectCreateOptionData(
            ParticleBulletMakerWeaponData particleBulletMakerWeaponData,
            Func<IPositionData> getFromPositionData,
            Func<Quaternion> getOffsetRotation,
            IPositionData targetData)
        {
            ParticleBulletMakerWeaponData = particleBulletMakerWeaponData;
            GetFromPositionData = getFromPositionData;

            // FIXME struct 消す そもそもこのRotation自体いらない
            GetOffsetRotation = getOffsetRotation;
            TargetData = targetData;
        }
    }
}
