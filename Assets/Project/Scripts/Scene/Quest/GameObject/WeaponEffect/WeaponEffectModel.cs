namespace AloneSpace
{
    public class WeaponEffectModel : GameObjectModel<WeaponEffectGameObjectHandler>
    {
        protected override WeaponEffectGameObjectHandler OnInit(IPositionData positionData)
        {
            return new WeaponEffectGameObjectHandler();
        }
    }
}
