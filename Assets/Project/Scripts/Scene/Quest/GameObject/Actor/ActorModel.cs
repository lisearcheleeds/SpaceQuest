using System;
using UnityEngine;

namespace AloneSpace
{
    public class ActorModel : MonoBehaviour
    {
        public Transform[] WeaponHolder => weaponHolder;

        public Bounds Bounds
        {
            get
            {
                bounds ??= CalculateBounds();
                return bounds.Value;
            }
        }

        [SerializeField] Transform[] weaponHolder;

        IPositionData positionData;
        Bounds? bounds;

        public void Init(IPositionData positionData)
        {
            this.positionData = positionData;
        }

        public ActorFeedback GetActorFeedback()
        {
            return new ActorFeedback();
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
    }
}
