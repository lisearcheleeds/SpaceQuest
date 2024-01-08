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
        List<Coroutine> currentCoroutines = new List<Coroutine>();
        AreaData observeArea;

        public void Initialize(QuestData questData)
        {
            this.questData = questData;

            MessageBus.Instance.User.SetObserveArea.AddListener(SetUserObserveArea);
        }

        public void Finalize()
        {
            MessageBus.Instance.User.SetObserveArea.RemoveListener(SetUserObserveArea);

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

                Refresh();
            }
        }

        void SetUserObserveArea(AreaData areaData)
        {
            observeArea = areaData;
            isDirty = true;
        }

        void Refresh()
        {
            currentCoroutines.Clear();

            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                Destroy(loadedPlacedObject.gameObject);
            }

            loadedPlacedObjects.Clear();

            if (ambientObject == null)
            {
                currentCoroutines.Add(
                    AssetLoader.Instance.StartLoadAsync<Transform>(
                        questData.StarSystemData.AmbientObjectAsset,
                        target => Instantiate(target, ambientObjectParent)));
            }

            var placedObjectAsset = observeArea?.PlacedObjectAsset;
            if (placedObjectAsset != null)
            {
                currentCoroutines.Add(
                    AssetLoader.Instance.StartLoadAsync<Transform>(
                        placedObjectAsset,
                        target => loadedPlacedObjects.Add(Instantiate(target, placedObjectParent))));
            }

            // エリアの周辺のオブジェクトの位置調整
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                // FIXME: 複数個対応
                loadedPlacedObject.localPosition = Vector3.zero;
            }
        }
    }
}
