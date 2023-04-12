using System;

namespace AloneSpace
{
    public class BulletMakerWeaponOrderModule : IOrderModule
    {
        BulletMakerWeaponData weaponData;
        
        public BulletMakerWeaponOrderModule(BulletMakerWeaponData weaponData)
        {
            this.weaponData = weaponData;
        }

        public void OnUpdateModule(float deltaTime)
        {
            if (0 < weaponData.WeaponStateData.FireTime)
            {
                // 連射中
                weaponData.WeaponStateData.FireTime = Math.Max(0, weaponData.WeaponStateData.FireTime - deltaTime);
            }

            if (0 < weaponData.WeaponStateData.ReloadTime)
            {
                // リロード中
                weaponData.WeaponStateData.ReloadTime = Math.Max(0, weaponData.WeaponStateData.ReloadTime - deltaTime);
            }

            if (weaponData.IsExecutable(weaponData.WeaponStateData.TargetData))
            {
                MessageBus.Instance.ExecuteTriggerWeapon.Broadcast(weaponData, weaponData.WeaponStateData.TargetData, 1.0f);
                
                weaponData.WeaponStateData.ResourceIndex++;
                weaponData.WeaponStateData.FireTime += weaponData.ParameterVO.FireRate;
            }

            weaponData.WeaponStateData.TargetData = null;
        }
    }
}