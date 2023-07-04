using System;
using UnityEngine;

namespace AloneSpace
{
    public class ActorModel : GameObjectModel<ActorGameObjectHandler>
    {
        protected override bool IsUseBounds => true;

        [SerializeField] Transform[] weaponHolders;

        protected override ActorGameObjectHandler OnInit(IPositionData positionData)
        {
            return new ActorGameObjectHandler(weaponHolders, MeshBounds.max.magnitude);
        }
    }
}
