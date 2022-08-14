using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SimpleNavMesh;
using UnityEngine;
using UnityEngine.Serialization;

namespace RoboQuest.Quest.InSide
{
    public class AreaAmbientController : MonoBehaviour
    {
        [SerializeField] NavMeshController navMeshController;
        [SerializeField] Transform ambientObjectParent;
        [SerializeField] Transform placedObjectParent;
        Transform ambientObject;
        Transform placedObject;
        
        public void Initialize()
        {
        }

        public void Finalize()
        {
        }

        public void ResetArea()
        {
            if (ambientObject != null)
            {
                Destroy(ambientObject.gameObject);
                ambientObject = null;
            }

            if (placedObject != null)
            {
                Destroy(placedObject.gameObject);
                placedObject = null;
            }
        }

        public IEnumerator LoadArea(QuestData questData, int areaIndex)
        {
            var areaAssetVO = questData.MapData.AreaData[areaIndex].AreaAssetVO;
            
            yield return new ParallelCoroutine(
                AssetLoader.LoadAsync<Transform>(areaAssetVO.AmbientObjectAsset, target => ambientObject = Instantiate(target, ambientObjectParent)),
                AssetLoader.LoadAsync<Transform>(areaAssetVO.PlacedObjectAsset, target => placedObject = Instantiate(target, placedObjectParent)));

            yield return navMeshController.GenerateNavMesh();
        }
    }
}
