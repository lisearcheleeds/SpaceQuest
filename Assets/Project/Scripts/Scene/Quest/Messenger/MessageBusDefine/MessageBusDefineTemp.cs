using System;

namespace AloneSpace
{
    public static class MessageBusDefineTemp
    {
        public class RegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}

        public class NoticeCollisionEventData : MessageBusBroadcaster<CollisionEventData>{}
        public class NoticeCollisionEventEffectData : MessageBusBroadcaster<CollisionEventEffectData>{}
        public class NoticeDamageEventData : MessageBusBroadcaster<DamageEventData>{}
        public class NoticeBrokenActorEventData : MessageBusBroadcaster<BrokenActorEventData>{}
    }
}
