using UnityEngine;

namespace AloneSpace
{
    public class ActorRelationData
    {
        public ActorData OtherActorData { get; }

        public Vector3 RelativePosition { get; }
        public float Distance { get; }

        public ActorRelationData(ActorData from, ActorData other)
        {
            OtherActorData = other;

            RelativePosition = other.Position - from.Position;
            Distance = RelativePosition.magnitude;
        }
    }
}
