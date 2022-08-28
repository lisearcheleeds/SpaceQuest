using UnityEngine;

namespace AloneSpace
{
    public class CameraAngleControllerEffect : MonoBehaviour
    {
        [SerializeField] Transform cameraAnchor;
      
        public void Initialize()
        {
            MessageBus.Instance.UserCommandSetCameraAngle.AddListener(UserCommandSetCameraAngle);
        }

        void UserCommandSetCameraAngle(Quaternion quaternion)
        {
            cameraAnchor.rotation = quaternion;
        }
    }
}
