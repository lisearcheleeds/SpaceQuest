namespace AloneSpace
{
    public interface IThinkModuleHolder : IModuleHolder
    { 
        IThinkModule ThinkModule { get; }
    }
}