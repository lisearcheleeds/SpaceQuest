using System;

namespace AloneSpace
{
    public class MissileMakerWeaponOrderModule : IOrderModule
    {
        MissileMakerWeaponData weaponData;
        
        public MissileMakerWeaponOrderModule(MissileMakerWeaponData weaponData)
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
                return;
            }

            if (weaponData.IsExecutable(weaponData.TargetData))
            {
                MessageBus.Instance.ExecuteTriggerWeapon.Broadcast(weaponData, weaponData.TargetData, 1.0f);
                
                weaponData.WeaponStateData.ResourceIndex++;
                weaponData.WeaponStateData.FireTime += weaponData.ParameterVO.LaunchIntervalTime;
            }

            weaponData.TargetData = null;
        }
    }
}