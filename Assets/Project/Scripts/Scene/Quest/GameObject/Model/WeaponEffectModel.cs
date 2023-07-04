namespace AloneSpace
{
    public class WeaponEffectModel : GameObjectModel<WeaponEffectGameObjectHandler>
    {
        protected override bool IsUseBounds => false;

        protected override WeaponEffectGameObjectHandler OnInit(IPositionData positionData)
        {
            return new WeaponEffectGameObjectHandler();
        }
    }
}
