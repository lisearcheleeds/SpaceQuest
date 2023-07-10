using UnityEngine;

namespace AloneSpace
{
    public class BulletWeaponEffectCreateOptionData : IWeaponEffectCreateOptionData
    {
        public WeaponData WeaponData => BulletMakerWeaponData;

        public BulletMakerWeaponData BulletMakerWeaponData { get; }
        public int? AreaId { get; }
        public Vector3 Position { get; }
        public Quaternion Rotation { get; }
        public IPositionData TargetData { get; }

        public BulletWeaponEffectCreateOptionData(
            BulletMakerWeaponData bulletMakerWeaponData,
            IPositionData fromPositionData,
            Quaternion rotation,
            IPositionData targetData)
        {
            BulletMakerWeaponData = bulletMakerWeaponData;
            AreaId = fromPositionData.AreaId;
            Position = fromPositionData.Position;
            Rotation = rotation;
            TargetData = targetData;
        }
    }
}
