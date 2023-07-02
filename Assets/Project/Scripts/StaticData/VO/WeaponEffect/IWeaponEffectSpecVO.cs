namespace AloneSpace
{
    /// <summary>
    /// 子にIWeaponEffectSpecVOやGraphicEffectSpecVOを持たないこと
    /// ※IWeaponSpecVOに集約する
    /// </summary>
    public interface IWeaponEffectSpecVO
    {
        int Id { get; }

        CacheableGameObjectPath Path { get; }

        WeaponEffectType WeaponEffectType { get; }

        float BaseDamage { get; }
    }
}
