﻿using System;
using System.Linq;

namespace AloneSpace
{
    /// <summary>
    /// 基本スペックと品質
    /// </summary>
    public class ActorSpecVO
    {
        public int Id => row.Id;
        public string Name => row.Name;
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
        public int WeaponSlotCount => WeaponSlotLayout.Length;
        public (float PositionX, float PositionY)[] WeaponSlotLayout { get; }

        // ブースター
        public float MainBoosterPower => row.MainBoosterPower;
        public float SubBoosterPower => row.SubBoosterPower;
        public float MaxSpeed => row.MaxSpeed;
        public float SpeedAttenuation => row.SpeedAttenuation;
        public float PitchRotatePower => row.PitchRotatePower;
        public float YawRotatePower => row.YawRotatePower;
        public float RollRotatePower => row.RollRotatePower;
        public float RotateAttenuationRatio => row.RotateAttenuationRatio;
        public float PitchRotateAttenuation => row.PitchRotateAttenuation;
        public float YawRotateAttenuation => row.YawRotateAttenuation;
        public float RollRotateAttenuation => row.RollRotateAttenuation;

        // インベントリ
        public int CapacityWidth => row.CapacityWidth;
        public int CapacityHeight => row.CapacityHeight;

        // 距離
        public float VisionSensorDistance => row.VisionSensorDistance;
        public float RadarSensorPerformance => row.RadarSensorPerformance;

        // SpecialEffect
        public SpecialEffectSpecVO[] SpecialEffectSpecVOs { get; }

        ActorSpecMaster.Row row;

        public ActorSpecVO(int id) : this(id, ActorQualityType.Default, 1.0f)
        {
        }

        public ActorSpecVO(int id, ActorQualityType qualityType, float quality)
        {
            row = ActorSpecMaster.Instance.Get(id);
            BrokenActorGraphicEffectSpecVO = new GraphicEffectSpecVO(row.BrokenActorGraphicEffectSpecMasterId);
            BrokenActorSmokeGraphicEffectSpecVO = new GraphicEffectSpecVO(ConstantId.BrokenActorSmokeGraphicEffectId);

            var weaponSlotLayouts = ActorWeaponSlotLayoutMaster.Instance.GetRange(id);
            WeaponSlotLayout = Enumerable.Range(0, weaponSlotLayouts.Length).Select(index =>
            {
                var layout = weaponSlotLayouts.FirstOrDefault(l => l.WeaponSlotIndex == index);
                return layout != default ? (layout.PositionX, layout.PositionY) : default;
            }).ToArray();

            var specialEffectMasterRows = ActorSpecSpecialEffectRelationMaster.Instance.GetRange(id);
            SpecialEffectSpecVOs = specialEffectMasterRows.Select(x => new SpecialEffectSpecVO(x.SpecialEffectId)).ToArray();
        }
    }
}
