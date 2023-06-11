namespace AloneSpace
{
    /// <summary>
    /// TODO: InstanceIdで管理したい気持ちある
    /// </summary>
    public class CollisionEventEffectData
    {
        public CollisionEventEffectSenderModule SenderModule { get; }
        public CollisionEventEffectReceiverModule ReceiverModule { get; }

        /// <summary>
        /// 衝突したモジュールのイベント
        /// Sender側が作成するルールにするけど、HashSet使うので2重に登録される問題ないはず
        /// </summary>
        /// <param name="senderModule"></param>
        /// <param name="receiverModule"></param>
        public CollisionEventEffectData(CollisionEventEffectSenderModule senderModule, CollisionEventEffectReceiverModule receiverModule)
        {
            SenderModule = senderModule;
            ReceiverModule = receiverModule;
        }
    }
}
