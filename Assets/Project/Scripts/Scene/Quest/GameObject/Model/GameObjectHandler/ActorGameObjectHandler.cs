using UnityEngine;

namespace AloneSpace
{
    public class ActorGameObjectHandler : IGameObjectHandler
    {
        public Transform[] WeaponHolders { get; }
        public float BoundingSize { get; }

        public ActorGameObjectHandler(Transform[] weaponHolders, float boundingSize)
        {
            WeaponHolders = weaponHolders;
            BoundingSize = boundingSize;
        }
    }
}
