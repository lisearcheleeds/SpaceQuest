namespace AloneSpace
{
    public static class MessageBusDefineInventory
    {
        public class PickItem : MessageBusBroadcaster<InventoryData, ItemInteractData>{}
        public class OnPickItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class DropItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class OnDropItem : MessageBusBroadcaster<InventoryData, ItemData>{}
        public class TransferItem : MessageBusBroadcaster<InventoryData, InventoryData, ItemData>{}
    }
}