﻿using System.Linq;
using UnityEngine;

namespace AloneSpace
{
    public abstract class GameObjectModel<T> : MonoBehaviour where T : IGameObjectHandler
    {
        public Bounds MeshBounds { get; private set; }
        protected abstract bool IsUseBounds { get; }

        [SerializeField] ModelCollider[] colliders;
        CollisionEventModule collisionEventModule;

        public T Init(IPositionData positionData)
        {
            return Init(positionData, null);
        }

        public T Init(IPositionData positionData, CollisionEventModule collisionEventModule)
        {
            this.collisionEventModule = collisionEventModule;

            for (var i = 0; i < colliders.Length; i++)
            {
                colliders[i].Init(collisionEventModule, i, OnPartsTriggerEnter, OnPartsTriggerStay, OnPartsTriggerExit);
            }

            if (IsUseBounds)
            {
                MeshBounds = CalculateBounds();
            }

            return OnInit(positionData);
        }

        protected abstract T OnInit(IPositionData positionData);

        void OnPartsTriggerEnter(int colliderIndex, Collider other)
        {
            if (CheckOurCollider(other))
            {
                return;
            }

            var otherModelCollider = other.gameObject.GetComponent<ModelCollider>();

            if (otherModelCollider == null)
            {
                Debug.LogError($"衝突相手にModelColliderがついてません {other.gameObject.name}");
                return;
            }

            if (otherModelCollider.CollisionEventModule == null)
            {
                Debug.LogError($"衝突相手のCollisionにイベントがついてません {otherModelCollider.gameObject.name}");
                return;
            }

            MessageBus.Instance.Temp.NoticeCollisionEventData.Broadcast(new CollisionEventData(collisionEventModule, otherModelCollider.CollisionEventModule));
        }

        void OnPartsTriggerStay(int colliderIndex, Collider other)
        {
            if (CheckOurCollider(other))
            {
                return;
            }

            var otherModelCollider = other.gameObject.GetComponent<ModelCollider>();
            MessageBus.Instance.Temp.NoticeCollisionEventData.Broadcast(new CollisionEventData(collisionEventModule, otherModelCollider.CollisionEventModule));
        }

        void OnPartsTriggerExit(int colliderIndex, Collider other)
        {
            /*
            if (CheckOurCollider(other))
            {
                return;
            }

            var otherModelCollider = other.gameObject.GetComponent<ModelCollider>();
            MessageBus.Instance.NoticeCollisionEventData.Broadcast(new CollisionEventData(collisionDataHolder, otherModelCollider.CollisionDataHolder));
            */
        }

        Bounds CalculateBounds()
        {
            var meshFilters = GetComponentsInChildren<MeshFilter>();
            var newBounds = new Bounds();
            foreach (var meshFilter in meshFilters)
            {
                newBounds.Encapsulate(meshFilter.mesh.bounds);
            }

            return newBounds;
        }

        bool CheckOurCollider(Collider other)
        {
            if (colliders.Any(c => c.CurrentCollider == other))
            {
                return true;
            }

            return false;
        }
    }
}
