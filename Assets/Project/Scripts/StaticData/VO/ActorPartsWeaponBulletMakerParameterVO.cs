using AloneSpace;

namespace AloneSpace
{
    public class ActorPartsWeaponBulletMakerParameterVO : IActorPartsWeaponParameterVO
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
            
        // 精度 1.0f以上
        public float Accuracy => row.Accuracy; 

        // 速度
        public float Speed => row.Speed; 

        ActorPartsWeaponRifleParameterMaster.Row row;
        WeaponEffectObjectPathMaster.Row projectilePath;
        
        public ActorPartsWeaponBulletMakerParameterVO(int id)
        {
            row = ActorPartsWeaponRifleParameterMaster.Instance.Get(id);
            projectilePath = WeaponEffectObjectPathMaster.Instance.Get(row.WeaponEffectAssetId);
        }
    }
}