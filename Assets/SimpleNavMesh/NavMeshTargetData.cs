using UnityEngine;
using UnityEngine.AI;

namespace SimpleNavMesh
{
    public class NavMeshTargetData
    {
        public NavMeshBuildSource NavMeshBuildSource { get; }
        public Bounds Bounds { get; }

        public NavMeshTargetData(NavMeshBuildSource navMeshBuildSource, Bounds bounds)
        {
            this.NavMeshBuildSource = navMeshBuildSource;
            this.Bounds = bounds;
        }
    }
}