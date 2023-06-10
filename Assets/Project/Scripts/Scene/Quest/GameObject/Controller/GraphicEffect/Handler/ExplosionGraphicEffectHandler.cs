namespace AloneSpace
{
    public class ExplosionGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData { get; }

        public ExplosionGraphicEffectHandler(IPositionData positionData)
        {
            PositionData = positionData;
        }
    }
}