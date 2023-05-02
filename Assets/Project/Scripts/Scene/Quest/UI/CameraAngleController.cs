using UnityEngine;
using UnityEngine.EventSystems;

namespace AloneSpace
{
    public class CameraAngleController : MonoBehaviour, IDragHandler
    {
        public void Initialize()
        {
        }

        public void Finalize()
        {
        }

        public void OnDrag(PointerEventData eventData)
        {
            MessageBus.Instance.UserCommandRotateCamera.Broadcast(eventData.delta);
        }
    }
}
