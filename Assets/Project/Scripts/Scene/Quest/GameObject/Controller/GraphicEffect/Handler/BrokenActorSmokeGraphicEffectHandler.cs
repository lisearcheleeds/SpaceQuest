namespace AloneSpace
{
    public class BrokenActorSmokeGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData { get; }

        public bool IsAbandoned { get; private set; }

        public void Abandon() => IsAbandoned = true;

        public BrokenActorSmokeGraphicEffectHandler(IPositionData positionData)
        {
            PositionData = positionData;
        }
    }
}
