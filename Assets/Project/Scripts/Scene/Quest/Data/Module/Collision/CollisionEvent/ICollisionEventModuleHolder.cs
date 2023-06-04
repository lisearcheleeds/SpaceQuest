using System;

namespace AloneSpace
{
    public interface ICollisionEventModuleHolder : IModuleHolder
    {
        Guid InstanceId { get; }
        CollisionEventModule CollisionEventModule { get; }
    }
}