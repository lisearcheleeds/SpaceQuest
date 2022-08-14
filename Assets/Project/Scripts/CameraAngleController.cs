using UnityEngine;
using UnityEngine.EventSystems;

namespace AloneSpace
{
    public class CameraAngleController : MonoBehaviour, IDragHandler
    {
        [SerializeField] Transform makerObject;
        [SerializeField] Transform cameraObject;

        public void OnDrag(PointerEventData eventData)
        {
            makerObject.rotation =
                Quaternion.AngleAxis(eventData.delta.magnitude, Vector3.Cross(Vector3.back, eventData.delta)) *
                makerObject.rotation;
            cameraObject.rotation = Quaternion.Inverse(makerObject.rotation);
        }
    }
}
