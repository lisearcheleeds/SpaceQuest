﻿using SimpleNavMesh;
using UnityEngine;
using UnityEngine.AI;

namespace RoboQuest.Quest.InSide
{
    public class ActorPathFinder
    {
        public bool HasPath => navMeshPathTracer.HasPath;
        public Vector3? NextPosition => nextShortcutPosition ?? nextPosition;
        public bool IsNextEndPosition => NextPosition == endPosition;
        
#if UNITY_EDITOR
        public Vector3[] Corners => navMeshPathTracer.Corners;
        public Vector3 EndPosition => endPosition;
#endif
        
        NavMeshPathTracer navMeshPathTracer;
        
        Vector3 endPosition;

        Vector3? nextPosition;
        Vector3? nextShortcutPosition;
        
        float tolerance;
        float actorWidth;
        
        public ActorPathFinder(int areaMask = NavMesh.AllAreas, float tolerance = 1.0f, float actorWidth = 2.0f)
        {
            this.tolerance = tolerance;
            this.actorWidth = actorWidth;
            navMeshPathTracer = new NavMeshPathTracer(areaMask, tolerance);
        }

        public void CalculatePath(Vector3 startPosition, Vector3 endPosition)
        {
            navMeshPathTracer.CalculatePath(startPosition, endPosition);
            this.endPosition = endPosition;
        }

        public void Update(Vector3 currentPosition)
        {
            if (!HasPath)
            {
                return;
            }

            if (nextShortcutPosition.HasValue && (nextShortcutPosition.Value - currentPosition).sqrMagnitude < tolerance)
            {
                CalculatePath(currentPosition, endPosition);
                nextShortcutPosition = null;
                return;
            }

            if (navMeshPathTracer.UpdatePathIndex(currentPosition))
            {
                // 目的地へのコーナーを曲がる度にショートカットのチェックをする
                nextShortcutPosition = GetShortcutPosition(currentPosition, endPosition);
                return;
            }

            nextPosition = navMeshPathTracer.NextPosition;
        }

        Vector3? GetShortcutPosition(Vector3 currentPosition, Vector3 endPosition)
        {
            var layerMask = 1 << LayerMask.NameToLayer("Terrain");
            
            var directRay = new Ray(currentPosition + Vector3.up * actorWidth, (endPosition - currentPosition).normalized);
            if (!Physics.Raycast(directRay, layerMask))
            {
                // 直進可能
                return endPosition;
            }
            
            // 迂回路
            return null;
        }
    }
}