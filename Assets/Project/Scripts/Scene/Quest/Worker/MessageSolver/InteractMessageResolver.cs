using System;
using System.Collections;
using System.Linq;
using AloneSpace;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AloneSpace
{
    public class InteractMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.Inventory.PickItem.AddListener(ManagerCommandPickItem);
            MessageBus.Instance.Inventory.DropItem.AddListener(ManagerCommandDropItem);
            MessageBus.Instance.Inventory.TransferItem.AddListener(TransferItem);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.Inventory.PickItem.RemoveListener(ManagerCommandPickItem);
            MessageBus.Instance.Inventory.DropItem.RemoveListener(ManagerCommandDropItem);
            MessageBus.Instance.Inventory.TransferItem.RemoveListener(TransferItem);
            MessageBus.Instance.Temp.NoticeCollisionEventEffectData.RemoveListener(NoticeCollisionEffectData);
        }

        void ManagerCommandPickItem(InventoryData toInventory, ItemInteractData pickItem)
        {
            var insertableId = toInventory.Inventory.GetInsertableId(pickItem.ItemData);
            if (!insertableId.HasValue)
            {
                // InteractItem.InteractItemを確認
                throw new ObjectDisposedException("ObjectDisposedException");
            }

            // Areaからアイテムを削除
            MessageBus.Instance.Data.ReleaseInteractData.Broadcast(pickItem);

            // インベントリにアイテムを追加
            toInventory.Inventory.InsertInventoryItem(insertableId.Value, pickItem.ItemData);
            
            MessageBus.Instance.Inventory.OnPickItem.Broadcast(toInventory, pickItem.ItemData);
        }

        void ManagerCommandDropItem(InventoryData fromInventory, ItemData dropItem)
        {
            var id = fromInventory.Inventory.GetId(dropItem);
            if (!id.HasValue)
            {
                // FIXME: 本当は必要だけどGridViewのPrePickで削除済みなのでそっちを治すまでは暫定で許す
                // throw new ArgumentException("DropしようとしたアイテムがInventory内に存在しません");
            }
            
            // インベントリからアイテムを削除
            fromInventory.Inventory.RemoveInventoryItem(id.Value);

            // Areaにアイテムを追加
            var offsetPosition = new Vector3(Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f), Random.Range(-50.0f, 50.0f));
            MessageBus.Instance.Data.CreateItemInteractData.Broadcast(
                dropItem,
                questData.UserData.ControlActorData.AreaId.Value,
                questData.UserData.ControlActorData.Position + offsetPosition,
                Quaternion.identity);
            
            MessageBus.Instance.Inventory.OnDropItem.Broadcast(fromInventory, dropItem);
        }

        void TransferItem(InventoryData fromInventory, InventoryData toInventory, ItemData itemData)
        {
            var removableId = fromInventory.Inventory.GetId(itemData);
            var insertableId = toInventory.Inventory.GetInsertableId(itemData);
            if (removableId.HasValue && insertableId.HasValue)
            {
                // アイテムを格納
                toInventory.Inventory.InsertInventoryItem(insertableId.Value, itemData);
                fromInventory.Inventory.RemoveInventoryItem(removableId.Value);
            }
            else
            {
                // InteractItem.InteractItemを確認
                throw new ObjectDisposedException("ObjectDisposedException");
            }
        }

        void NoticeCollisionEffectData(CollisionEventEffectData collisionEventEffectData)
        {
            /*
            if (!(damageableData is ActorData actorData))
            {
                return;
            }

            var areaData = questData.StarSystemData.AreaData.First(areaData => areaData.AreaId == actorData.AreaId);

            // 一覧から削除
            questData.ActorData.Remove(actorData.InstanceId);

            // 残骸を設置
            var interactBrokenActorData = new BrokenActorInteractData(actorData);
            areaData.AddInteractData(interactBrokenActorData);

            // 適当なアイテムを設置
            var inventoryData = ItemDataVOHelper.GetActorDropInventoryData(actorData);
            areaData.AddInteractData(new InventoryInteractData(inventoryData, actorData.AreaId.Value, actorData.Position, actorData.Rotation));

            // 更新
            MessageBus.Instance.SetDirtyActorObjectList.Broadcast();
            */
        }
    }
}
