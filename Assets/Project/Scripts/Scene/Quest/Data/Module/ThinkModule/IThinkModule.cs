using System;

namespace AloneSpace
{
    public interface IThinkModule : IModule
    {
        Guid InstanceId { get; }
    }
}