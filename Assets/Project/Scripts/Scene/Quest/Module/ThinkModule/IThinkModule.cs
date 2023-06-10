using System;

namespace AloneSpace
{
    /// <summary>
    /// 思考処理
    /// 毎フレーム呼び出されない
    /// </summary>
    public interface IThinkModule : IModule
    {
        Guid InstanceId { get; }
    }
}