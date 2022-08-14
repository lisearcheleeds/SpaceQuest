using System;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace SimpleNavMesh
{
    public class NavMeshPathTracer
    {
        public bool HasPath => corners != null && corners.Length != 0;
        public Vector3? NextPosition => HasPath ? corners[cornerIndex] : (Vector3?)null;

        public Vector3[] Corners => corners;
        
        float tolerance;
        int areaMask;
        
        NavMeshPath navMeshPath;

        Vector3[] corners;
        int cornerIndex;

        public NavMeshPathTracer(int areaMask, float tolerance)
        {
            navMeshPath = new NavMeshPath();
            this.areaMask = areaMask;
            this.tolerance = tolerance;
        }

        public void CalculatePath(Vector3 startPosition, Vector3 endPosition)
        {
            cornerIndex = 0;

            if (NavMesh.CalculatePath(startPosition, endPosition, areaMask, navMeshPath))
            {
                corners = navMeshPath.corners;
                return;
            }
            
            NavMeshHit navMeshHit;
            if (NavMesh.SamplePosition(startPosition, out navMeshHit, 10000, areaMask))
            {
                if (NavMesh.CalculatePath(navMeshHit.position, endPosition, areaMask, navMeshPath))
                {
                    corners = new[] { navMeshHit.position }.Concat(navMeshPath.corners).ToArray();
                }
            }
        }

        public bool UpdatePathIndex(Vector3 currentPosition)
        {
            if (corners == null)
            {
                cornerIndex = 0;
                return true;
            }

            var prevCornerIndex = cornerIndex;
            
            while (cornerIndex < corners.Length)
            {
                // 次の地点への距離
                var nextRelativePosition = corners[cornerIndex] - currentPosition;
                nextRelativePosition.y = 0;
                if (nextRelativePosition.sqrMagnitude < tolerance)
                {
                    cornerIndex++;
                }
                else
                {
                    if (cornerIndex + 1 < corners.Length)
                    {
                        var moreNextRelativePosition = corners[cornerIndex + 1] - currentPosition;
                        moreNextRelativePosition.y = 0;
                        
                        var nextToMoreNextRelativePosition = corners[cornerIndex + 1] - currentPosition;
                        nextToMoreNextRelativePosition.y = 0;
                        
                        if (moreNextRelativePosition.sqrMagnitude < nextToMoreNextRelativePosition.sqrMagnitude)
                        {
                            // 次の地点のほうが近ければ一度だけindexを進める
                            cornerIndex++;
                        }
                    }
                    
                    break;
                }
            }

            return prevCornerIndex != cornerIndex;
        }
    }
}
