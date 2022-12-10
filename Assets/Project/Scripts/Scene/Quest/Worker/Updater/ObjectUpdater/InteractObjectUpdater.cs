using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace AloneSpace
{
    public class InteractObjectUpdater : IUpdater
    {
        List<InteractionObject> interactionObjectList = new List<InteractionObject>();
        
        QuestData questData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
        }

        public void OnLateUpdate()
        {
        }
        
        public IEnumerator LoadArea()
        {
            return RefreshInteractObject();
        }

        public IEnumerator RefreshInteractObject()
        {
            var interactData = questData.ObserveAreaData.InteractData;
            
            // 不要なオブジェクトを消す
            foreach (var interactionObject in interactionObjectList.ToArray())
            {
                interactionObject.Release();
                interactionObjectList.Remove(interactionObject);
            }
            
            // 必要なオブジェクトを作る
            var waitCount = 0;
            var waitCounter = 0;
            foreach (var data in interactData)
            {
                waitCount++;
                CreateInteractObject(questData, data, () => waitCounter++);
            }

            yield return new WaitWhile(() => waitCount != waitCounter);
        }

        void CreateInteractObject(QuestData questData, IInteractData interactData, Action onCreate = null)
        {
            switch (interactData)
            {
                case ItemInteractData interactItemData:
                    GameObjectCache.Instance.GetAsset<ItemObject>(
                        ConstantAssetPath.ItemObjectPathVO,
                        itemObject =>
                        {
                            itemObject.Apply(interactItemData); 
                            itemObject.IsActive = true;
                            interactionObjectList.Add(itemObject);
                            onCreate?.Invoke();
                        });
                    break;
                
                case BrokenActorInteractData interactBrokenActorData:
                    GameObjectCache.Instance.GetAsset<BrokenActorObject>(
                        ConstantAssetPath.BrokenActorObjectPathVO,
                        brokenActorObject =>
                        {
                            brokenActorObject.Apply(interactBrokenActorData); 
                            brokenActorObject.IsActive = true;
                            interactionObjectList.Add(brokenActorObject);
                            onCreate?.Invoke();
                        });
                    break;
                
                case InventoryInteractData inventoryInteractData:
                    GameObjectCache.Instance.GetAsset<InventoryObject>(
                        ConstantAssetPath.InventoryObjectPathVO,
                        inventoryObject =>
                        {
                            inventoryObject.Apply(inventoryInteractData); 
                            inventoryObject.IsActive = true;
                            interactionObjectList.Add(inventoryObject);
                            onCreate?.Invoke();
                        });
                    break;
            } 
        }
    }
}
