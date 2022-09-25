using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class InteractObjectUpdater : IUpdater
    {
        bool isDirty = false;
        int areaIndex;
        List<IInteractData> interactList = new List<IInteractData>();
        
        public void Initialize()
        {
            MessageBus.Instance.UpdateInteractData.AddListener(UpdateInteractData);
            MessageBus.Instance.SubscribeUpdateAll.AddListener(SubscribeUpdateAll);
        }

        public void Finalize()
        {
            MessageBus.Instance.UpdateInteractData.RemoveListener(UpdateInteractData);
            MessageBus.Instance.SubscribeUpdateAll.RemoveListener(SubscribeUpdateAll);
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                MessageBus.Instance.SubscribeUpdateInteractionObjectList.Broadcast(interactList.ToArray());
                isDirty = false;
            }
        }
        
        public void ResetArea()
        {
            isDirty = true;
        }
        
        public IEnumerator LoadArea(QuestData questData, int areaIndex)
        {
            this.areaIndex = areaIndex;
            var waitCount = questData.MapData.AreaData[areaIndex].InteractData.Count;
            var waitCounter = 0;
            
            foreach (var interactData in questData.MapData.AreaData[areaIndex].InteractData)
            {
                CreateInteractData(interactData, () =>
                {
                    waitCounter++;
                });
            }

            yield return new WaitWhile(() => waitCount != waitCounter);
        }

        public void OnLoadedArea()
        {
            isDirty = true;
        }

        void UpdateInteractData(int areaIndex, IInteractData[] interactData)
        {
            // DataとObjectをあわせる
            if (this.areaIndex != areaIndex)
            {
                return;
            }

            foreach (var createTarget in interactData)
            {
                // 無ければ作る
                if (interactList.All(x => x.InstanceId != createTarget.InstanceId))
                {
                    CreateInteractData(createTarget, () => isDirty = true);
                }
            }

            foreach (var deleteTarget in interactList.ToArray())
            {
                // 不要なので消す
                if (interactData.All(x => x.InstanceId != deleteTarget.InstanceId))
                {
                    ((InteractionObject)deleteTarget).Release();
                    interactList.Remove(deleteTarget);
                    isDirty = true;
                }
            }
        }
        
        void CreateInteractData(IInteractData interactData, Action onCreate = null)
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
                            onCreate?.Invoke();
                        });
                    break;
            } 
        }

        void SendInteractionObject(IInteractData interactData, bool isEntry)
        {
            if (isEntry)
            {
                interactList.Add(interactData);
            }
            else
            {
                interactList.Remove(interactData);
            }

            isDirty = true;
        }

        void SubscribeUpdateAll()
        {
            isDirty = true;
        }
    }
}
