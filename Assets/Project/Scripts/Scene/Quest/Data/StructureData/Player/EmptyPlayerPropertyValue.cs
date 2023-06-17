namespace AloneSpace
{
    public class EmptyPlayerPropertyValue : IPlayerPropertyValue
    {
        public static EmptyPlayerPropertyValue Empty { get; } = new EmptyPlayerPropertyValue();
    }
}
