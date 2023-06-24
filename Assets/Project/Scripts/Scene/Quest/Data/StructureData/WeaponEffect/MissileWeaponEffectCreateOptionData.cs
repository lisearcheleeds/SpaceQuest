using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData { get; }
        public int? AreaId { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public IPositionData TargetData { get; }

        public Vector3 LaunchMovementVelocity { get; }

        public MissileWeaponEffectCreateOptionData(
            WeaponData weaponData,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData,
            Vector3 launchMovementVelocity)
        {
            WeaponData = weaponData;
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;
            TargetData = targetData;

            LaunchMovementVelocity = launchMovementVelocity;
        }
    }
}
