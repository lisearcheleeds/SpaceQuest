namespace AloneSpace
{
    public interface IReleasableData
    {
        bool IsReleased { get; }
        void Release();
    }
}
