﻿using System;
using System.Collections;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InteractMessageResolver
    {
        QuestData questData;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.ManagerCommandPickItem.AddListener(PickItem);
            MessageBus.Instance.ManagerCommandTransferItem.AddListener(TransferItem);
            MessageBus.Instance.NoticeCollisionEventEffectData.AddListener(NoticeCollisionEffectData);
        }

        public void Finalize()
        {
            MessageBus.Instance.ManagerCommandPickItem.RemoveListener(PickItem);
            MessageBus.Instance.ManagerCommandTransferItem.RemoveListener(TransferItem);
            MessageBus.Instance.NoticeCollisionEventEffectData.RemoveListener(NoticeCollisionEffectData);
        }

        void PickItem(InventoryData toInventory, ItemInteractData pickItem)
        {
            var insertableId = toInventory.Inventory.GetInsertableId(pickItem.ItemData);
            if (insertableId.HasValue)
            {
                // アイテムを格納
                toInventory.Inventory.InsertInventoryItem(insertableId.Value, pickItem.ItemData);

                MessageBus.Instance.ReleaseInteractData.Broadcast(pickItem);
            }
            else
            {
                // InteractItem.InteractItemを確認
                throw new ObjectDisposedException("ObjectDisposedException");
            }
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
