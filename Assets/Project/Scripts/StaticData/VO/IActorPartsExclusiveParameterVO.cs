namespace RoboQuest
{ 
    /// <summary>
    /// パーツの拡張データ
    /// nullの場合は画面に表示しない
    /// </summary>
    public interface IActorPartsExclusiveParameterVO : IActorPartsParameterVO
    {
        // ID
        int Id { get; }

        // 拡張パラメータタイプ
        ActorPartsExclusiveType ActorPartsExclusiveType { get; }
    }
}