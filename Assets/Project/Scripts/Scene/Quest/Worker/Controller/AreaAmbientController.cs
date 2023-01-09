﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEditor.VersionControl;
using UnityEngine;

namespace AloneSpace
{
    public class AreaAmbientController : MonoBehaviour
    {
        [SerializeField] Transform ambientObjectParent;
        [SerializeField] Transform placedObjectParent;
        
        Transform ambientObject;
        QuestData questData;

        List<Transform> loadedPlacedObjects = new List<Transform>();

        bool isDirty;
        Coroutine currentCoroutine;
        AreaData currentAreaData;
        
        public void Initialize(QuestData questData)
        {
            this.questData = questData;
        }

        public void Finalize()
        {
            if (ambientObject != null)
            {
                Destroy(ambientObject.gameObject);
                ambientObject = null;
            }
        }

        public void OnLateUpdate()
        {
            if (isDirty)
            {
                isDirty = false;
            
                if (currentCoroutine != null)
                {
                    StopCoroutine(currentCoroutine);
                }

                currentCoroutine = StartCoroutine(Refresh());
            }
        }

        public void SetObserveAreaData(AreaData areaData)
        {
            currentAreaData = areaData;
            isDirty = true;
        }

        IEnumerator Refresh()
        {
            var coroutines = new List<IEnumerator>();

            if (ambientObject == null)
            {
                coroutines.Add(AssetLoader.LoadAsync<Transform>(
                    questData.StarSystemData.AmbientObjectAsset,
                    target => Instantiate(target, ambientObjectParent)));
            }
            
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                Destroy(loadedPlacedObject.gameObject);
            }
            
            loadedPlacedObjects.Clear();

            var placedObjectAsset = currentAreaData?.PlacedObjectAsset;
            if (placedObjectAsset != null)
            {
                coroutines.Add(AssetLoader.LoadAsync<Transform>(
                    placedObjectAsset,
                    target => loadedPlacedObjects.Add(Instantiate(target, placedObjectParent))));
            }
            
            yield return new ParallelCoroutine(coroutines);
            
            // これどこかに移動
            MessageBus.Instance.UserCommandSetAmbientCameraPosition.Broadcast(currentAreaData.StarSystemPosition);
            
            // エリアの周辺のオブジェクトの位置調整
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                // FIXME: 複数個対応
                loadedPlacedObject.localPosition = Vector3.zero;
            }
        }
    }
}
