namespace AloneSpace
{
    /// <summary>
    /// 子にIWeaponEffectSpecVOやGraphicEffectSpecVOを持たないこと
    /// </summary>
    public class GraphicEffectSpecVO
    {
        public CacheableGameObjectPath Path => row.Path;

        GraphicEffectSpecMaster.Row row;

        public GraphicEffectSpecVO(int id)
        {
            row = GraphicEffectSpecMaster.Instance.Get(id);
        }
    }
}