namespace AloneSpace
{
    public class WeaponFeedback
    {
        public IPositionData[] OutputPositionData { get; }

        public WeaponFeedback(IPositionData[] outputPositionData)
        {
            OutputPositionData = outputPositionData;
        }
    }
}