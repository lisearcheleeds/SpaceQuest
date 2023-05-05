﻿namespace AloneSpace
{
    /// <summary>
    /// 毎フレーム呼び出される基本処理
    /// </summary>
    public interface IOrderModuleHolder : IModuleHolder
    {
        IOrderModule OrderModule { get; }
    }
}