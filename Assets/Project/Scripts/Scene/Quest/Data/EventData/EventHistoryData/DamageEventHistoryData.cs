using System;

namespace AloneSpace
{
    /// <summary>
    /// ダメージ発生後のログデータ
    /// 無限に溜まってくログデータなので注意
    /// GC意識する
    /// </summary>
    public class DamageEventHistoryData
    {
        public Guid WeaponDataInstanceId { get; }
        public Guid WeaponHolderInstanceId { get; }
        public Guid WeaponEffectDataInstanceId { get; }
        public Guid DamagedActorDataInstanceId { get; }

        public int WeaponSpecId { get; }
        public int WeaponHolderSpecId { get; }
        public int WeaponEffectSpecId { get; }
        public int DamagedActorDataSpecId { get; }

        public float EffectedDamageValue { get; }
        public float DecayDamageValue { get; }
        public float ShieldDamage { get; }
        public float EnduranceDamage { get; }
        public float OverKillDamage { get; }

        public bool IsLethalDamage { get; }
        public bool IsShieldBreakDamage => ShieldDamage != 0 && EnduranceDamage != 0;
        public bool IsEnduranceDamage => EnduranceDamage != 0;

        public WeaponEffectType WeaponEffectType { get; }

        public DamageEventHistoryData(
            DamageEventData damageEventData,
            float decayDamageValue,
            float shieldDamage,
            float enduranceDamage,
            float overKillDamage,
            bool isLethalDamage)
        {
            WeaponDataInstanceId = damageEventData.WeaponData.InstanceId;
            WeaponHolderInstanceId = damageEventData.WeaponData.WeaponHolder.InstanceId;
            WeaponEffectDataInstanceId = damageEventData.WeaponEffectData.InstanceId;
            DamagedActorDataInstanceId = damageEventData.DamagedActorData.InstanceId;

            WeaponSpecId = damageEventData.WeaponData.WeaponSpecVO.Id;
            WeaponHolderSpecId = damageEventData.WeaponData.WeaponHolder.ActorSpecVO.Id;
            WeaponEffectSpecId = damageEventData.WeaponEffectData.WeaponEffectSpecVO.Id;
            DamagedActorDataSpecId = damageEventData.DamagedActorData.ActorSpecVO.Id;

            EffectedDamageValue = damageEventData.EffectedDamageValue;
            DecayDamageValue = decayDamageValue;
            ShieldDamage = shieldDamage;
            EnduranceDamage = enduranceDamage;
            OverKillDamage = overKillDamage;

            IsLethalDamage = isLethalDamage;

            WeaponEffectType = damageEventData.WeaponEffectData.WeaponEffectSpecVO.WeaponEffectType;
        }
    }
}
