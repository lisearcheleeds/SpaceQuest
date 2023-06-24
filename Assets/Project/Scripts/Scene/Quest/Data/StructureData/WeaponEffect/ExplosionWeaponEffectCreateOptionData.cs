using UnityEngine;

namespace AloneSpace
{
    public class ExplosionWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData { get; }
        public int? AreaId { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }

        public ExplosionWeaponEffectCreateOptionData(
            WeaponData weaponData,
            IPositionData fromPositionData,
            Quaternion rotation)
        {
            WeaponData = weaponData;
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;
        }
    }
}
