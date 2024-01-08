namespace AloneSpace
{
    public static class MessageBusDefineModule
    {
        public class RegisterMovingModule : MessageBusBroadcaster<MovingModule>{}
        public class UnRegisterMovingModule : MessageBusBroadcaster<MovingModule>{}
        public class RegisterOrderModule : MessageBusBroadcaster<IOrderModule>{}
        public class UnRegisterOrderModule : MessageBusBroadcaster<IOrderModule>{}
        public class RegisterThinkModule : MessageBusBroadcaster<IThinkModule>{}
        public class UnRegisterThinkModule : MessageBusBroadcaster<IThinkModule>{}
        public class RegisterCollisionEventModule : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollisionEventModule : MessageBusBroadcaster<CollisionEventModule>{}
        public class RegisterCollisionEffectReceiverModuleModule : MessageBusBroadcaster<CollisionEventEffectReceiverModule>{}
        public class UnRegisterCollisionEffectReceiverModuleModule : MessageBusBroadcaster<CollisionEventEffectReceiverModule>{}
        public class RegisterCollisionEffectSenderModuleModule : MessageBusBroadcaster<CollisionEventEffectSenderModule>{}
        public class UnRegisterCollisionEffectSenderModuleModule : MessageBusBroadcaster<CollisionEventEffectSenderModule>{}
    }
}
