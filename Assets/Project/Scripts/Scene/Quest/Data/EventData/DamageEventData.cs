namespace AloneSpace
{
    /// <summary>
    /// ダメージ発生時のデータ
    /// TODO: InstanceIdで管理したい気持ちある
    /// </summary>
    public class DamageEventData
    {
        // Damageを与えた側
        public WeaponData WeaponData { get; }
        public WeaponEffectData WeaponEffectData { get; }

        // Damageを受けた側
        public ActorData DamagedActorData { get; }

        // 実際に受けたダメージ量
        public float DamageValue { get; }

        // 減衰したダメージ量
        public float DecayDamageValue { get; }

        public DamageEventData(
            WeaponData weaponData,
            WeaponEffectData weaponEffectData,
            ActorData damagedActorData,
            float damageValue,
            float decayDamageValue)
        {
            WeaponData = weaponData;
            WeaponEffectData = weaponEffectData;
            DamagedActorData = damagedActorData;
            DamageValue = damageValue;
            DecayDamageValue = decayDamageValue;
        }
    }
}
