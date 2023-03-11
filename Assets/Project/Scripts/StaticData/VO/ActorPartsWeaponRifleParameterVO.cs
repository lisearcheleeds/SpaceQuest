using AloneSpace;

namespace AloneSpace
{
    public class ActorPartsWeaponRifleParameterVO : IActorPartsWeaponParameterVO
    {
        // ID
        public int Id => row.Id;

        // 武器タイプ
        public WeaponType WeaponType => WeaponType.Rifle;

        // 射出物アセット
        public ICacheableGameObjectPath ProjectilePath => projectilePath;
            
        // マガジンサイズ
        public int WeaponResourceMaxCount => row.WeaponResourceMaxCount;
            
        // リロード時間
        public float ReloadTime => row.ReloadTime;

        // 連射速度(f/s)
        public float FireRate => row.FireRate; 
            
        // 精度(0.0 ~ 1.0f)
        public float Accuracy => row.Accuracy; 

        // 横反動抑制値(0.0 ~ 1.0f)
        public float HorizontalRecoilResist => row.HorizontalRecoilResist; 
            
        // 縦反動抑制値(0.0 ~ 1.0f)
        public float VerticalRecoilResist => row.VerticalRecoilResist; 

        // 照準最大距離（弾の性能に寄って下がる）
        public float SightingMaxRange => row.SightingMaxRange; 

        ActorPartsWeaponRifleParameterMaster.Row row;
        ProjectilePathMaster.Row projectilePath;
        
        public ActorPartsWeaponRifleParameterVO(int id)
        {
            row = ActorPartsWeaponRifleParameterMaster.Instance.Get(id);
            projectilePath = ProjectilePathMaster.Instance.Get(row.ProjectileAssetId);
        }
    }
}