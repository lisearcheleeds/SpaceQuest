using System.Collections;
using System.Collections.Generic;
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
        AreaData observeArea;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.SetUserObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.SetUserObserveArea.RemoveListener(SetUserObserveArea);

            if (ambientObject != null)
            {
                Destroy(ambientObject.gameObject);
                ambientObject = null;
            }
        }

        public void OnUpdate()
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

        void SetUserObserveArea(AreaData areaData)
        {
            observeArea = areaData;
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

            var placedObjectAsset = observeArea?.PlacedObjectAsset;
            if (placedObjectAsset != null)
            {
                coroutines.Add(AssetLoader.LoadAsync<Transform>(
                    placedObjectAsset,
                    target => loadedPlacedObjects.Add(Instantiate(target, placedObjectParent))));
            }

            yield return new ParallelCoroutine(coroutines);

            // エリアの周辺のオブジェクトの位置調整
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                // FIXME: 複数個対応
                loadedPlacedObject.localPosition = Vector3.zero;
            }
        }
    }
}
