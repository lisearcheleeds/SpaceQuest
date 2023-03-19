using UnityEngine;

namespace AloneSpace
{
    public class UserData
    {
        public PlayerQuestData PlayerQuestData { get; private set; }
        public AreaData CurrentAreaData { get; private set; }

        public Vector3 LookAtAngle { get; private set; } = Vector3.forward;
        public Quaternion LookAtSpace { get; private set; } = Quaternion.identity;

        public void SetPlayerQuestData(PlayerQuestData playerQuestData)
        {
            PlayerQuestData = playerQuestData;
        }

        public void SetCurrentAreaData(AreaData currentAreaData)
        {
            CurrentAreaData = currentAreaData;
        }

        public void SetLookAtAngle(Vector3 lookAtAngle)
        {
            LookAtAngle = lookAtAngle;
        }

        public void SetLookAtSpace(Quaternion lookAtSpace)
        {
            LookAtSpace = lookAtSpace;
        }
    }
}