using UnityEngine;

namespace AloneSpace
{
    public class ActorGameObjectHandler : IGameObjectHandler
    {
        public Transform[] WeaponHolders { get; }

        public ActorGameObjectHandler(Transform[] weaponHolders)
        {
            WeaponHolders = weaponHolders;
        }
    }
}