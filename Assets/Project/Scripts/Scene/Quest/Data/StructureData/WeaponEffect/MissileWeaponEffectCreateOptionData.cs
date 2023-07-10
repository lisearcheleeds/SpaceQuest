using UnityEngine;

namespace AloneSpace
{
    public class MissileWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData => MissileMakerWeaponData;

        public MissileMakerWeaponData MissileMakerWeaponData { get; }
        public int? AreaId { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public IPositionData TargetData { get; }

        public Vector3 LaunchMovementVelocity { get; }

        public MissileWeaponEffectCreateOptionData(
            MissileMakerWeaponData missileMakerWeaponData,
            IPositionData fromPositionData,
            IPositionData targetData,
            Vector3 launchMovementVelocity)
        {
            MissileMakerWeaponData = missileMakerWeaponData;
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = fromPositionData.Rotation;
            TargetData = targetData;

            LaunchMovementVelocity = launchMovementVelocity;
        }
    }
}
