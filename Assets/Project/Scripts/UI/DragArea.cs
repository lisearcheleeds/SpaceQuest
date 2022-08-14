using UnityEngine;
using UnityEngine.EventSystems;
using VariableInventorySystem;

namespace RoboQuest.Common
{
    public class DragArea : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] bool isLimited;
        [SerializeField] RectTransform target;

        public bool IsLimited => isLimited;

        protected virtual Vector3 DefaultPosition { get; set; }

        bool isInitialized;
        bool isCache;

        public void ResetPosition()
        {
            Initialize();
            target.localPosition = DefaultPosition;
        }

        void Awake()
        {
            Initialize();
        }

        void Update()
        {
        }

        void Initialize()
        {
            if (isInitialized)
            {
                return;
            }

            isInitialized = true;

            DefaultPosition = Vector3.zero;
            isCache = false;
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            isCache = true;
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (isCache)
            {
                target.localPosition = target.localPosition + new Vector3(eventData.delta.x, eventData.delta.y);

                if (IsLimited)
                {
                    var localPosition = target.localPosition;
                    if (localPosition.x < -target.sizeDelta.x * 0.5f)
                    {
                        localPosition.x = -target.sizeDelta.x * 0.5f;
                    }

                    if (localPosition.x > target.sizeDelta.x * 0.5f)
                    {
                        localPosition.x = target.sizeDelta.x * 0.5f;
                    }

                    if (localPosition.y < -target.sizeDelta.y * 0.5f)
                    {
                        localPosition.y = -target.sizeDelta.y * 0.5f;
                    }

                    if (localPosition.y > target.sizeDelta.y * 0.5f)
                    {
                        localPosition.y = target.sizeDelta.y * 0.5f;
                    }

                    target.localPosition = localPosition;
                }
            }
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            isCache = false;
        }
    }
}
