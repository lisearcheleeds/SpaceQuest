namespace AloneSpace
{
    public partial class MessageBus
    {
        public ModuleMessage Module { get; } = new ModuleMessage();

        public class ModuleMessage
        {
            public MessageBusDefineModule.RegisterMovingModule RegisterMovingModule { get; } = new MessageBusDefineModule.RegisterMovingModule();
            public MessageBusDefineModule.UnRegisterMovingModule UnRegisterMovingModule { get; } = new MessageBusDefineModule.UnRegisterMovingModule();

            public MessageBusDefineModule.RegisterOrderModule RegisterOrderModule { get; } = new MessageBusDefineModule.RegisterOrderModule();
            public MessageBusDefineModule.UnRegisterOrderModule UnRegisterOrderModule { get; } = new MessageBusDefineModule.UnRegisterOrderModule();

            public MessageBusDefineModule.RegisterThinkModule RegisterThinkModule { get; } = new MessageBusDefineModule.RegisterThinkModule();
            public MessageBusDefineModule.UnRegisterThinkModule UnRegisterThinkModule { get; } = new MessageBusDefineModule.UnRegisterThinkModule();

            public MessageBusDefineModule.RegisterCollisionEventModule RegisterCollisionEventModule { get; } = new MessageBusDefineModule.RegisterCollisionEventModule();
            public MessageBusDefineModule.UnRegisterCollisionEventModule UnRegisterCollisionEventModule { get; } = new MessageBusDefineModule.UnRegisterCollisionEventModule();

            public MessageBusDefineModule.RegisterCollisionEffectReceiverModuleModule RegisterCollisionEffectReceiverModule { get; } = new MessageBusDefineModule.RegisterCollisionEffectReceiverModuleModule();
            public MessageBusDefineModule.UnRegisterCollisionEffectReceiverModuleModule UnRegisterCollisionEffectReceiverModule { get; } = new MessageBusDefineModule.UnRegisterCollisionEffectReceiverModuleModule();

            public MessageBusDefineModule.RegisterCollisionEffectSenderModuleModule RegisterCollisionEffectSenderModule { get; } = new MessageBusDefineModule.RegisterCollisionEffectSenderModuleModule();
            public MessageBusDefineModule.UnRegisterCollisionEffectSenderModuleModule UnRegisterCollisionEffectSenderModule { get; } = new MessageBusDefineModule.UnRegisterCollisionEffectSenderModuleModule();
        }
    }
}
