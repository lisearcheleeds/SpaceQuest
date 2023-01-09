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

        AreaData loadedAreaData;
        
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
        
        public IEnumerator LoadArea(AreaData areaData)
        {
            this.loadedAreaData = areaData;
            
            return RefreshInteractObject();
        }

        public IEnumerator RefreshInteractObject()
        {
            var interactData = loadedAreaData.InteractData;
            
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

        void CreateInteractObject(QuestData questData, IInteractData interactData, Action onComplete)
        {
            var assetPathVO = interactData switch
            {
                ItemInteractData _ => ConstantAssetPath.ItemObjectPathVO,
                BrokenActorInteractData _ => ConstantAssetPath.BrokenActorObjectPathVO,
                InventoryInteractData _ => ConstantAssetPath.InventoryObjectPathVO,
                AreaInteractData _ => null,
                _ => throw new NotImplementedException(),
            };

            if (assetPathVO == null)
            {
                onComplete();
                return;
            }

            GameObjectCache.Instance.GetAsset<InteractionObject>(
                assetPathVO,
                interactionObject =>
                {
                    interactionObject.SetInteractData(interactData); 
                    interactionObject.IsActive = true;
                    interactionObjectList.Add(interactionObject);
                    onComplete();
                });
        }
    }
}
