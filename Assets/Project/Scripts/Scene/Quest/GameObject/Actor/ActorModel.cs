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
        Bounds? bounds;

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
