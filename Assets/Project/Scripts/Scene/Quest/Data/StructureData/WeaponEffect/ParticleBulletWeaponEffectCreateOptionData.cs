using UnityEngine;

namespace AloneSpace
{
    public class ParticleBulletWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData { get; }
        public int? AreaId { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public IPositionData TargetData { get; }

        public ParticleBulletWeaponEffectCreateOptionData(
            WeaponData weaponData,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData)
        {
            WeaponData = weaponData;
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;
            TargetData = targetData;
        }
    }
}
