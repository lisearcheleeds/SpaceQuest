using System;

namespace AloneSpace
{
    public partial class MessageBusDefine
    {
        public class RegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}
        public class UnRegisterCollision : MessageBusBroadcaster<CollisionEventModule>{}

        public class SetUserPlayer : MessageBusBroadcaster<PlayerData>{}
        public class SetUserControlActor : MessageBusBroadcaster<ActorData>{}
        public class SetUserObserveTarget : MessageBusBroadcaster<IPositionData>{}

        public class SetUserObserveArea : MessageBusBroadcaster<AreaData>{}

        public class NoticeCollisionEventData : MessageBusBroadcaster<CollisionEventData>{}
        public class NoticeCollisionEventEffectData : MessageBusBroadcaster<CollisionEventEffectData>{}
        public class NoticeDamageEventData : MessageBusBroadcaster<DamageEventData>{}
        public class NoticeBrokenActorEventData : MessageBusBroadcaster<BrokenActorEventData>{}

        public class PlayerCommandSetAreaId : MessageBusBroadcaster<Guid, int?>{}
        public class PlayerCommandSetMoveTarget : MessageBusBroadcaster<Guid, IPositionData>{}
        public class PlayerCommandSetTacticsType : MessageBusBroadcaster<Guid, TacticsType>{}
        public class PlayerCommandAddInteractOrder : MessageBusBroadcaster<Guid, IInteractData>{}
        public class PlayerCommandRemoveInteractOrder : MessageBusBroadcaster<Guid, IInteractData>{}

        public class OnAddInteractOrder : MessageBusBroadcaster<Guid, InteractOrderState>{}
        public class OnRemoveInteractOrder : MessageBusBroadcaster<Guid, InteractOrderState>{}
        
        public class ManagerCommandPickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class ManagerCommandPickedItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class ManagerCommandDropItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class ManagerCommandDroppedItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class ManagerCommandTransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}
    }
}
