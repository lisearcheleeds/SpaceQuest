using UnityEngine;

namespace AloneSpace
{
    public class UserData
    {
        public PlayerData PlayerData { get; private set; }
        public AreaData CurrentAreaData { get; private set; }

        public ActorOperationMode ActorOperationMode { get; private set; } = ActorOperationMode.Observe;

        public Vector3 LookAtAngle { get; private set; } = Vector3.forward;
        public Quaternion LookAtSpace { get; private set; } = Quaternion.identity;
        public float LookAtDistance { get; private set; }

        public void SetPlayerData(PlayerData playerData)
        {
            PlayerData = playerData;
        }

        public void SetCurrentAreaData(AreaData currentAreaData)
        {
            CurrentAreaData = currentAreaData;
        }

        public void SetActorOperationMode(ActorOperationMode actorOperationMode)
        {
            ActorOperationMode = actorOperationMode;
        }

        public void SetLookAtAngle(Vector3 lookAtAngle)
        {
            LookAtAngle = lookAtAngle;
        }

        public void SetLookAtSpace(Quaternion lookAtSpace)
        {
            LookAtSpace = lookAtSpace;
        }

        public void SetLookAtDistance(float distance)
        {
            LookAtDistance = distance;
        }
    }
}
