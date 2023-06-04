namespace AloneSpace
{
    public class WeaponGameObjectHandler : IGameObjectHandler
    {
        public IPositionData[] OutputPositionData { get; }

        public WeaponGameObjectHandler(IPositionData[] outputPositionData)
        {
            OutputPositionData = outputPositionData;
        }
    }
}