using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AloneSpace
{
    public class InteractObjectUpdater : IUpdater
    {
        QuestData questData;
        
        MonoBehaviour coroutineWorker;
        Coroutine currentCoroutine;
        Transform variableParent;
        AreaData currentAreaData;
        bool isDirty;
        
        List<InteractionObject> interactionObjectList = new List<InteractionObject>();
        
        public void Initialize(QuestData questData, Transform variableParent, MonoBehaviour coroutineWorker)
        {
            this.questData = questData;
            this.variableParent = variableParent;
            this.coroutineWorker = coroutineWorker;
            
            MessageBus.Instance.SetDirtyInteractObjectList.AddListener(SetDirtyInteractObjectList);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetDirtyInteractObjectList.RemoveListener(SetDirtyInteractObjectList);
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
            
                if (currentCoroutine != null)
                {
                    coroutineWorker.StopCoroutine(currentCoroutine);
                }

                currentCoroutine = coroutineWorker.StartCoroutine(Refresh());
            }
            
            foreach (var interactionObject in interactionObjectList)
            {
                interactionObject.OnLateUpdate();
            }
        }
        
        public void SetObserveAreaData(AreaData areaData)
        {
            this.currentAreaData = areaData;
            SetDirtyInteractObjectList();
        }

        IEnumerator Refresh()
        {
            // 不要なオブジェクトを消す
            foreach (var interactionObject in interactionObjectList.ToArray())
            {
                interactionObject.Release();
                interactionObjectList.Remove(interactionObject);
            }
            
            if (currentAreaData == null)
            {
                yield break;
            }
            
            // 必要なオブジェクトを作る
            var waitCount = 0;
            var waitCounter = 0;
            foreach (var data in currentAreaData.InteractData)
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
                    interactionObject.transform.SetParent(variableParent, false);
                    interactionObjectList.Add(interactionObject);
                    onComplete();
                });
        }

        void SetDirtyInteractObjectList()
        {
            isDirty = true;
        }
    }
}
