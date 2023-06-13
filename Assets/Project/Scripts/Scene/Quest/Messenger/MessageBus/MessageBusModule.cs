namespace AloneSpace
{
    public partial class MessageBus
    {
        public MessageBusDefine.RegisterMovingModule RegisterMovingModule { get; } = new MessageBusDefine.RegisterMovingModule();
        public MessageBusDefine.UnRegisterMovingModule UnRegisterMovingModule { get; } = new MessageBusDefine.UnRegisterMovingModule();

        public MessageBusDefine.RegisterOrderModule RegisterOrderModule { get; } = new MessageBusDefine.RegisterOrderModule();
        public MessageBusDefine.UnRegisterOrderModule UnRegisterOrderModule { get; } = new MessageBusDefine.UnRegisterOrderModule();

        public MessageBusDefine.RegisterThinkModule RegisterThinkModule { get; } = new MessageBusDefine.RegisterThinkModule();
        public MessageBusDefine.UnRegisterThinkModule UnRegisterThinkModule { get; } = new MessageBusDefine.UnRegisterThinkModule();

        public MessageBusDefine.RegisterCollisionEventModule RegisterCollisionEventModule { get; } = new MessageBusDefine.RegisterCollisionEventModule();
        public MessageBusDefine.UnRegisterCollisionEventModule UnRegisterCollisionEventModule { get; } = new MessageBusDefine.UnRegisterCollisionEventModule();

        public MessageBusDefine.RegisterCollisionEffectReceiverModuleModule RegisterCollisionEffectReceiverModule { get; } = new MessageBusDefine.RegisterCollisionEffectReceiverModuleModule();
        public MessageBusDefine.UnRegisterCollisionEffectReceiverModuleModule UnRegisterCollisionEffectReceiverModule { get; } = new MessageBusDefine.UnRegisterCollisionEffectReceiverModuleModule();

        public MessageBusDefine.RegisterCollisionEffectSenderModuleModule RegisterCollisionEffectSenderModule { get; } = new MessageBusDefine.RegisterCollisionEffectSenderModuleModule();
        public MessageBusDefine.UnRegisterCollisionEffectSenderModuleModule UnRegisterCollisionEffectSenderModule { get; } = new MessageBusDefine.UnRegisterCollisionEffectSenderModuleModule();
    }
}
