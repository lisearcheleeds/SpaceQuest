namespace AloneSpace
{
    /// <summary>
    /// 子にIWeaponEffectSpecVOやGraphicEffectSpecVOを持たないこと
    /// ※IWeaponSpecVOに集約する
    /// </summary>
    public interface IWeaponEffectSpecVO
    {
        CacheableGameObjectPath Path { get; }
    }
}