using UnityEngine;

namespace AloneSpace
{
    public struct ActorRelationData
    {
        public ActorData OtherActorData { get; private set; }

        public Vector3 RelativePosition { get; private set; }
        public float SqrDistance { get; private set; }

        public void Update(ActorData from, ActorData other)
        {
            OtherActorData = other;

            RelativePosition = other.Position - from.Position;
            SqrDistance = RelativePosition.sqrMagnitude;
        }
    }
}
