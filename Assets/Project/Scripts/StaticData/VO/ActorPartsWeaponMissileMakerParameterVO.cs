﻿namespace AloneSpace
{
    public class ActorPartsWeaponMissileMakerParameterVO : IActorPartsWeaponParameterVO
    {
        // ID
        public int Id => row.Id;

        // 武器タイプ
        public WeaponType WeaponType => WeaponType.MissileLauncher;

        // 射出物アセット
        public ICacheableGameObjectPath ProjectilePath => projectilePath;
        
        // 一度に使用するリソース
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;
            
        // リロード時間
        public float ReloadTime => row.ReloadTime; 
            
        // 発射時の方向X
        // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
        public float? LaunchAngleX => row.LaunchAngleX;
            
        // 発射時の方向Y
        // 設定されているとtargetの位置にかかわらずshooterの方向とBaseAngleに依存して射出する
        public float? LaunchAngleY => row.LaunchAngleY;
            
        // 射出初速
        public float LaunchSpeed => row.LaunchSpeed;
            
        // 連続射出時間
        public float FireRate => row.FireRate;
        
        // 照準最大距離（弾の性能に寄って下がる）
        public float SightingMaxRange => row.SightingMaxRange; 

        ActorPartsWeaponMissileLauncherParameterMaster.Row row;
        WeaponEffectObjectPathMaster.Row projectilePath;
        
        public ActorPartsWeaponMissileMakerParameterVO(int id)
        {
            row = ActorPartsWeaponMissileLauncherParameterMaster.Instance.Get(id);
            projectilePath = WeaponEffectObjectPathMaster.Instance.Get(row.WeaponEffectAssetId);
        }
    }
}