namespace AloneSpace
{
    public abstract class WeaponEffect : CacheableGameObject
    {
        public abstract WeaponEffectData WeaponEffectData { get; }
        
        public abstract void SetWeaponEffectData(WeaponEffectData weaponEffectData);
        public abstract void OnLateUpdate();
    }
}