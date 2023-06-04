using UnityEngine;

namespace AloneSpace
{
    public abstract class WeaponEffect : CacheableGameObject
    {
        public abstract WeaponEventEffectData WeaponEventEffectData { get; }
        public WeaponEffectModel WeaponEffectModel => weaponEffectModel;

        [SerializeField] WeaponEffectModel weaponEffectModel;

        public void Init(WeaponEventEffectData weaponEventEffectData)
        {
            weaponEventEffectData.SetWeaponEffectGameObjectHandler(weaponEffectModel.Init(weaponEventEffectData, weaponEventEffectData.CollisionEventModule));
            OnInit(weaponEventEffectData);
        }

        protected abstract void OnInit(WeaponEventEffectData weaponEventEffectData);
        public abstract void OnLateUpdate();
    }
}