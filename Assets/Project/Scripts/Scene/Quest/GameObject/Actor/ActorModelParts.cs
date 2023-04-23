using UnityEngine;

namespace AloneSpace
{
    public class ActorModelParts : MonoBehaviour
    {
        public int PartsId { get; private set; }
        public Bounds Bounds { get; private set; }

        public void Initialize(int partsId)
        {
            this.PartsId = partsId;
        }
        
        public void UpdateBounds()
        {
            var colliders = GetComponentsInChildren<Collider>();
            var bounds = new Bounds();

            foreach (var collider in colliders)
            {
                bounds.Encapsulate(collider.bounds.extents);
                bounds.Encapsulate(-collider.bounds.extents);
            }

            Bounds = bounds;
        }
        
        public void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawCube(transform.position, Bounds.size);
        }
    }
}
