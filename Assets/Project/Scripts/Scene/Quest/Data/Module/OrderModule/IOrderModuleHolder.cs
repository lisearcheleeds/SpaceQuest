namespace AloneSpace
{
    public interface IOrderModuleHolder : IModuleHolder
    {
        IOrderModule OrderModule { get; }
    }
}