using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RoboQuest;
using UnityEngine;

namespace AloneSpace.InSide
{
    public class ActorModel : MonoBehaviour
    {
        public float Height => bounds.size.y;
        public float OffsetHeight => Height * 0.5f;

        Bounds bounds;
        ActorModelParts[] actorModelParts;

        public IEnumerator LoadActorModel(ActorSpecData actorSpecData)
        {
            var coroutines = new List<IEnumerator>();
            var results = new List<ActorModelParts>();

            var root = new GameObject("RootParts").AddComponent<ActorModelParts>();
            root.Initialize(0);
            root.transform.SetParent(transform, false);
            results.Add(root);
            
            // ツリー状のID特殊な読み込み
            foreach (var actorPartsVO in actorSpecData.ActorPartsVOs)
            {
                coroutines.Add(LoadParts(actorPartsVO, results, actorSpecData.ActorBluePrint));
            }

            yield return new ParallelCoroutine(coroutines);

            actorModelParts = results.ToArray();
            UpdateBounds();
            RefreshOffsetHeight();
        }
        
        static IEnumerator LoadParts(ActorPartsVO actorPartsVO, List<ActorModelParts> parentsCandidate, ActorBluePrint actorBluePrint)
        {
            GameObject loadAsset = null;
            yield return AssetLoader.LoadAsync(actorPartsVO, prefab => loadAsset = prefab);

            var parentPartId = actorBluePrint.PartsHierarchy.First(kv => kv.Value.Contains(actorPartsVO.Id)).Key;
            while (!parentsCandidate.Exists(x => x.PartsId == parentPartId))
            {
                yield return null;
            }
            
            var instantiateObject = Instantiate(loadAsset, parentsCandidate.First(x => x.PartsId == parentPartId).transform, false).GetComponent<ActorModelParts>();
            instantiateObject.Initialize(actorPartsVO.Id);
            parentsCandidate.Add(instantiateObject);
        }

        void UpdateBounds()
        {
            var bounds = new Bounds();

            foreach (var actorPart in actorModelParts)
            {
                actorPart.UpdateBounds();
                
                bounds.Encapsulate(actorPart.Bounds.extents);
                bounds.Encapsulate(-actorPart.Bounds.extents);
            }

            this.bounds = bounds;
        }

        void RefreshOffsetHeight()
        {
            transform.localPosition = Vector3.up * OffsetHeight;
        }

        void OnDrawGizmosSelected()
        {
            foreach (var actorPart in actorModelParts)
            {
                actorPart.OnDrawGizmosSelected();
            }
        }
    }
}
