namespace AloneSpace
{
    /// <summary>
    /// TODO: InstanceIdで管理したい
    /// </summary>
    public class DamageEventData
    {
        // Damageを与える側
        public WeaponData WeaponData { get; }
        public WeaponEffectData WeaponEffectData { get; }

        // Damageを受ける側
        public ActorData DamagedActorData { get; }

        // 要求するダメージ量
        public float EffectedDamageValue { get; }

        public DamageEventData(
            WeaponData weaponData,
            WeaponEffectData weaponEffectData,
            ActorData damagedActorData,
            float effectedDamageValue)
        {
            WeaponData = weaponData;
            WeaponEffectData = weaponEffectData;
            DamagedActorData = damagedActorData;
            EffectedDamageValue = effectedDamageValue;
        }
    }
}
