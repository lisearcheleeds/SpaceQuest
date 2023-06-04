using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEffect : CacheableGameObject
    {
        public abstract WeaponEffectData WeaponEffectData { get; }
        public WeaponEffectModel WeaponEffectModel => weaponEffectModel;

        [SerializeField] WeaponEffectModel weaponEffectModel;

        public void Init(WeaponEffectData weaponEffectData)
        {
            weaponEffectData.SetWeaponEffectGameObjectHandler(weaponEffectModel.Init(weaponEffectData, weaponEffectData.CollisionEventModule));
            OnInit(weaponEffectData);
        }

        protected abstract void OnInit(WeaponEffectData weaponEffectData);
        public abstract void OnLateUpdate();
    }
}