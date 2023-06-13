using System;
using UnityEngine;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class RegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}

        public class SetOrderUserPlayer : MessageBusBroadcaster<Guid>{}
        public class SetUserPlayer : MessageBusBroadcaster<PlayerData>{}
        public class SetOrderUserArea : MessageBusBroadcaster<int?>{}
        public class SetUserArea : MessageBusBroadcaster<AreaData>{}

        public class NoticeCollisionEventData : MessageBusBroadcaster<CollisionEventData>{}
        public class NoticeCollisionEventEffectData : MessageBusBroadcaster<CollisionEventEffectData>{}
        public class NoticeDamageEventData : MessageBusBroadcaster<DamageEventData>{}
        public class NoticeBrokenActorEventData : MessageBusBroadcaster<BrokenActorEventData>{}

        public class PlayerCommandSetAreaId : MessageBusBroadcaster<ActorData, int?>{}
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<ActorData, IPositionData>{}
        public class PlayerCommandSetInteractOrder : MessageBusBroadcaster<ActorData, IInteractData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}

        public class ManagerCommandPickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}
    }
}
