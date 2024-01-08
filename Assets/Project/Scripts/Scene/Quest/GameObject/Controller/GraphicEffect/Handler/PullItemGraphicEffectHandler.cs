namespace AloneSpace
{
    public class PullItemGraphicEffectHandler : IGraphicEffectHandler
    {
        public IPositionData PositionData { get; }
        public IPositionData ItemPositionData { get; }

        public bool IsAbandoned { get; private set; }

        public void Abandon() => IsAbandoned = true;

        public PullItemGraphicEffectHandler(IPositionData positionData, IPositionData itemPositionData)
        {
            PositionData = positionData;
            ItemPositionData = itemPositionData;
        }
    }
}