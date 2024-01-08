namespace AloneSpace
{
    public partial class MessageBus
    {
        public InventoryMessage Inventory { get; } = new InventoryMessage();

        public class InventoryMessage
        {
            public MessageBusDefineInventory.PickItem PickItem { get; } = new MessageBusDefineInventory.PickItem();
            public MessageBusDefineInventory.OnPickItem OnPickItem { get; } = new MessageBusDefineInventory.OnPickItem();
            public MessageBusDefineInventory.DropItem DropItem { get; } = new MessageBusDefineInventory.DropItem();
            public MessageBusDefineInventory.OnDropItem OnDropItem { get; } = new MessageBusDefineInventory.OnDropItem();
            public MessageBusDefineInventory.TransferItem TransferItem { get; } = new MessageBusDefineInventory.TransferItem();
        }
    }
}
