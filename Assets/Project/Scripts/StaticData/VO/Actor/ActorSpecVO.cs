namespace AloneSpace
{
    /// <summary>
    /// Actorの基本スペックと品質
    /// </summary>
    public class ActorSpecVO
    {
        public int Id => row.Id;
        public AssetPath Path => row.Path;

        public GraphicEffectSpecVO BrokenActorGraphicEffectSpecVO { get; }
        public GraphicEffectSpecVO BrokenActorSmokeGraphicEffectSpecVO { get; }

        // 耐久
        public float EnduranceValue => row.EnduranceValue;

        public float ShieldValue => row.ShieldValue;
        public float ShieldTruncateValue => row.ShieldTruncateValue;
        public float ShieldAutoRecoveryResilienceTime => row.ShieldAutoRecoveryResilienceTime;
        public float ShieldAutoRecoveryValue => row.ShieldAutoRecoveryValue;

        public float ElectronicProtectionValue => row.ElectronicProtectionValue;
        public float ElectronicProtectionTruncateValue => row.ElectronicProtectionTruncateValue;
        public float ElectronicProtectionAutoRecoveryResilienceTime => row.ElectronicProtectionAutoRecoveryResilienceTime;
        public float ElectronicProtectionAutoRecoveryValue => row.ElectronicProtectionAutoRecoveryValue;

        // 武器
        public int WeaponSlotCount => row.WeaponSlotCount;

        // ブースター
        public float MainBoosterPower => row.MainBoosterPower;
        public float SubBoosterPower => row.SubBoosterPower;
        public float MaxSpeed => row.MaxSpeed;
        public float PitchBoosterPower => row.RotatePower;
        public float YawBoosterPower => row.RotatePower;
        public float RollBoosterPower => row.RotatePower;

        // インベントリ
        public int CapacityWidth => row.CapacityWidth;
        public int CapacityHeight => row.CapacityHeight;

        // 距離
        public float VisionSensorDistance => row.VisionSensorDistance;
        public float RadarSensorPerformance => row.RadarSensorPerformance;

        ActorSpecMaster.Row row;

        public ActorSpecVO(int id) : this(id, ActorQualityType.Default, 1.0f)
        {
        }

        public ActorSpecVO(int id, ActorQualityType qualityType, float quality)
        {
            row = ActorSpecMaster.Instance.Get(id);
            BrokenActorGraphicEffectSpecVO = new GraphicEffectSpecVO(row.BrokenActorGraphicEffectSpecMasterId);
            BrokenActorSmokeGraphicEffectSpecVO = new GraphicEffectSpecVO(ConstantId.BrokenActorSmokeGraphicEffectId);
        }
    }
}
