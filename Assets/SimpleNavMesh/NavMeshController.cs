using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleNavMesh
{
    public class NavMeshController : MonoBehaviour
    {
        List<INavMeshTarget> navMeshTargets = new List<INavMeshTarget>();

        NavMeshData navMeshData;
        NavMeshDataInstance navMeshDataInstance;

        [SerializeField] GameObject navMeshTargetParent;

        public IEnumerator GenerateNavMesh()
        {
            Reset();

            navMeshData = new NavMeshData();
            navMeshDataInstance = NavMesh.AddNavMeshData(navMeshData);
            navMeshTargets = navMeshTargetParent.GetComponentsInChildren<INavMeshTarget>().ToList();

            foreach (var navMeshTarget in navMeshTargets)
            {
                navMeshTarget.UpdateNavMeshTargetData();
            }

            return UpdateNavMeshData();
        }

        public void AddNavMesh(INavMeshTarget navMeshTarget)
        {
            navMeshTargets.Add(navMeshTarget);
        }

        public IEnumerator UpdateNavMeshData()
        {
            var defaultBuildSettings = Enumerable.Range(0, NavMesh.GetSettingsCount())
                .Select(i => NavMesh.GetSettingsByIndex(i))
                .FirstOrDefault(x => NavMesh.GetSettingsNameFromID(x.agentTypeID) == "Actor");
            defaultBuildSettings.agentHeight = 2.0f;
            defaultBuildSettings.agentClimb = 2.0f;
            defaultBuildSettings.agentRadius = 2.0f;
            var sources = navMeshTargets.Select(x => x.NavMeshTargetData.NavMeshBuildSource).ToList();
            var operation = NavMeshBuilder.UpdateNavMeshDataAsync(navMeshData, defaultBuildSettings, sources, QuantizedBounds());

            yield return operation;
        }

        public void Reset()
        {
            navMeshTargets.Clear();
            navMeshDataInstance.Remove();
        }

        Bounds QuantizedBounds()
        {
            var bounds = new Bounds();

            foreach (var navMeshTarget in navMeshTargets)
            {
                bounds.Encapsulate(navMeshTarget.NavMeshTargetData.Bounds.max);
                bounds.Encapsulate(navMeshTarget.NavMeshTargetData.Bounds.min);
            }

            return bounds;
        }

        void OnDrawGizmosSelected()
        {
            if (navMeshData)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireCube(navMeshData.sourceBounds.center, navMeshData.sourceBounds.size);
            }

            Gizmos.color = Color.yellow;
            var bounds = QuantizedBounds();
            Gizmos.DrawWireCube(bounds.center, bounds.size);
        }
    }
}
