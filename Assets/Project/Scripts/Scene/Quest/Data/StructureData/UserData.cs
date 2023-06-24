using UnityEngine;

namespace AloneSpace
{
    public class UserData
    {
        public PlayerData PlayerData { get; private set; }
        public ActorData ControlActorData { get; private set; }
        public IPositionData ObserveTarget { get; private set; }
        public AreaData ObserveAreaData { get; private set; }

        public ActorOperationMode ActorOperationMode { get; private set; } = ActorOperationMode.Observe;

        public Vector3 LookAtAngle { get; private set; } = Vector3.zero;
        public Quaternion LookAtSpace { get; private set; } = Quaternion.identity;
        public float LookAtDistance { get; private set; }

        public void SetPlayerData(PlayerData playerData)
        {
            PlayerData = playerData;
        }

        public void SetControlActorData(ActorData controlActorData)
        {
            ControlActorData = controlActorData;
        }

        public void SetObserveTarget(IPositionData observeTarget)
        {
            ObserveTarget = observeTarget;
        }

        public void SetObserveAreaData(AreaData observeAreaData)
        {
            ObserveAreaData = observeAreaData;
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
