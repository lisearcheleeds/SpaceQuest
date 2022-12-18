using System.Collections;
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

        public IEnumerator LoadArea()
        {
            var coroutines = new List<IEnumerator>();

            if (ambientObject == null)
            {
                coroutines.Add(AssetLoader.LoadAsync<Transform>(
                    questData.StarSystemData.AmbientObjectAsset,
                    target => Instantiate(target, ambientObjectParent)));
            }
            
            // 次のエリアに引き継がないオブジェクトを削除
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                Destroy(loadedPlacedObject);
            }
            
            loadedPlacedObjects.Clear();

            // 次のエリアで新規生成する必要があるオブジェクトを生成
            coroutines.Add(AssetLoader.LoadAsync<Transform>(
                questData.ObserveAreaData.PlacedObjectAsset,
                target => loadedPlacedObjects.Add(Instantiate(target, placedObjectParent))));
            
            yield return new ParallelCoroutine(coroutines);
            
            MessageBus.Instance.UserCommandSetAmbientCameraPosition.Broadcast(questData.ObserveAreaData.Position);
            
            // エリアの周辺のオブジェクトの位置調整
            foreach (var loadedPlacedObject in loadedPlacedObjects)
            {
                // FIXME: 複数個対応
                loadedPlacedObject.localPosition = Vector3.zero;
            }
        }
    }
}
