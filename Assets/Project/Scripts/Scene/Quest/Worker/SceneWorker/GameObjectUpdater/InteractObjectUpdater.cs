using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public class InteractObjectUpdater
    {
        MonoBehaviour coroutineWorker;
        Coroutine currentCoroutine;
        AreaData observeArea;
        bool isDirty;

        List<InteractionObject> interactionObjectList = new List<InteractionObject>();

        QuestData questData;

        public void Initialize(QuestData questData, MonoBehaviour coroutineWorker)
        {
            this.questData = questData;
            this.coroutineWorker = coroutineWorker;

            MessageBus.Instance.Creator.OnCreateInteractData.AddListener(OnCreateInteractData);
            MessageBus.Instance.Creator.OnReleaseInteractData.AddListener(OnReleaseInteractData);
            MessageBus.Instance.Temp.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.Creator.OnCreateInteractData.RemoveListener(OnCreateInteractData);
            MessageBus.Instance.Creator.OnReleaseInteractData.RemoveListener(OnReleaseInteractData);
            MessageBus.Instance.Temp.SetUserObserveArea.RemoveListener(SetUserObserveArea);
        }

        public void OnUpdate()
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

        void SetUserObserveArea(AreaData areaData)
        {
            observeArea = areaData;
            SetDirtyInteractObjectList();
        }

        IEnumerator Refresh()
        {
            // 不要なオブジェクトを消す
            foreach (var interactionObject in interactionObjectList.ToArray())
            {
                ReleaseInteractObject(interactionObject);
            }

            if (observeArea == null)
            {
                yield break;
            }

            // 必要なオブジェクトを作る
            var waitCount = 0;
            var waitCounter = 0;
            if (observeArea != null)
            {
                var observeAreaInteractData = questData.InteractData.Values.Where(x => x.AreaId == observeArea.AreaId);
                foreach (var data in observeAreaInteractData)
                {
                    waitCount++;
                    CreateInteractObject(data, () => waitCounter++);
                }
            }

            yield return new WaitWhile(() => waitCount != waitCounter);
        }

        void CreateInteractObject(IInteractData interactData, Action onComplete)
        {
            var assetPathVO = interactData switch
            {
                ItemInteractData _ => ConstantAssetPath.ItemObjectPathVO,
                InventoryInteractData _ => ConstantAssetPath.InventoryObjectPathVO,
                AreaInteractData _ => null,
                _ => throw new NotImplementedException(),
            };

            if (assetPathVO == null)
            {
                onComplete?.Invoke();
                return;
            }

            MessageBus.Instance.GetCacheAsset.Broadcast(assetPathVO, c =>
            {
                var interactionObject = (InteractionObject)c;
                interactionObject.SetInteractData(interactData);
                interactionObjectList.Add(interactionObject);
                onComplete?.Invoke();
            });
        }

        void ReleaseInteractObject(InteractionObject interactionObject)
        {
            interactionObject.Release();
            interactionObjectList.Remove(interactionObject);
        }

        void SetDirtyInteractObjectList()
        {
            isDirty = true;
        }

        void OnCreateInteractData(IInteractData interactData)
        {
            if (interactData.AreaId == observeArea?.AreaId)
            {
                CreateInteractObject(interactData, null);
            }
        }

        void OnReleaseInteractData(IInteractData interactData)
        {
            if (interactData.AreaId == observeArea?.AreaId)
            {
                ReleaseInteractObject(interactionObjectList.First(x => x.InteractData.InstanceId == interactData.InstanceId));
            }
        }
    }
}
