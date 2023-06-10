namespace AloneSpace
{
    public class MissileGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData { get; }

        public bool IsAbandoned { get; private set; }

        public void Abandon() => IsAbandoned = true;

        public MissileGraphicEffectHandler(IPositionData positionData)
        {
            PositionData = positionData;
        }
    }
}