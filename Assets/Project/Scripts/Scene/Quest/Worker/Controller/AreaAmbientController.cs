using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AloneSpace;
using UnityEngine;

namespace AloneSpace
{
    public class AreaAmbientController : MonoBehaviour
    {
        [SerializeField] Transform ambientObjectParent;
        [SerializeField] Transform placedObjectParent;
        
        Transform ambientObject;

        Dictionary<int, (AreaDirection? AreaDirection, Transform Transform)> loadedPlacedObjects =
            new Dictionary<int, (AreaDirection? AreaDirection, Transform Transform)>();
        
        public void Initialize()
        {
        }

        public void Finalize()
        {
            if (ambientObject != null)
            {
                Destroy(ambientObject.gameObject);
                ambientObject = null;
            }
        }

        public IEnumerator LoadArea(QuestData questData)
        {
            List<IEnumerator> coroutines = new List<IEnumerator>();

            if (ambientObject != null)
            {
                coroutines.Add(AssetLoader.LoadAsync<Transform>(questData.MapData.AmbientObjectAsset, target => Instantiate(target, ambientObjectParent)));
            }
            
            // 次のエリアに引き継がないオブジェクトを削除
            foreach (var loadedAreaIndex in loadedPlacedObjects.Keys)
            {
                if (questData.ObserveAdjacentAreaData.All(adjacentAreaData => loadedAreaIndex != adjacentAreaData.AreaData.AreaIndex))
                {
                    Destroy(loadedPlacedObjects[loadedAreaIndex].Transform);
                    loadedPlacedObjects.Remove(loadedAreaIndex);
                }
            }

            // 次のエリアで新規生成する必要があるオブジェクトを生成
            foreach (var adjacentAreaData in questData.ObserveAdjacentAreaData)
            {
                if (!loadedPlacedObjects.ContainsKey(adjacentAreaData.AreaData.AreaIndex))
                {
                    coroutines.Add(AssetLoader.LoadAsync<Transform>(
                        questData.MapData.AreaData[adjacentAreaData.AreaData.AreaIndex].AreaAssetVO.PlacedObjectAsset,
                        target => loadedPlacedObjects.Add(adjacentAreaData.AreaData.AreaIndex, (adjacentAreaData.AreaDirection, Instantiate(target, placedObjectParent)))));
                }
            }
            
            yield return new ParallelCoroutine(coroutines);
            
            // エリアの周辺のオブジェクトの位置調整
            foreach (var loadedPlacedObjectValue in loadedPlacedObjects.Values)
            {
                var offset = loadedPlacedObjectValue.AreaDirection.HasValue
                    ? AreaCellVertex.GetVector(loadedPlacedObjectValue.AreaDirection.Value)
                    : Vector3.zero;

                loadedPlacedObjectValue.Transform.localPosition = offset * questData.MapData.AreaSize;
            }
        }
    }
}
