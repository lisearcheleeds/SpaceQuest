namespace AloneSpace
{
    public interface IDamageCollisionEventEffectSenderModule
    {
        public WeaponData WeaponData { get; }
        public WeaponEffectData WeaponEffectData { get; }

        // 実際に与えようとしているダメージ
        public float EffectedDamageValue { get; }
    }
}
