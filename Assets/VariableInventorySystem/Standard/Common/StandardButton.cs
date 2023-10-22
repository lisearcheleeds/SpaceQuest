using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace VariableInventorySystem
{
    public class StandardButton : Button, ICellActions
    {
        Action onPointerClick;
        Action onPointerOptionClick;
        Action onPointerEnter;
        Action onPointerExit;

        Coroutine longPointerCoroutine;

        public bool IsActive
        {
            get => interactable;
            set
            {
                interactable = value;
                foreach (var graphic in GetComponentsInChildren<Graphic>())
                {
                    graphic.raycastTarget = value;
                }
            }
        }

        public void SetCallback(Action onPointerClick)
        {
            this.onPointerClick = onPointerClick;
        }

        public void SetCallback(
            Action onPointerClick,
            Action onPointerOptionClick,
            Action onPointerEnter,
            Action onPointerExit)
        {
            this.onPointerClick = onPointerClick;
            this.onPointerOptionClick = onPointerOptionClick;
            this.onPointerEnter = onPointerEnter;
            this.onPointerExit = onPointerExit;
        }

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);

#if (UNITY_IOS || UNITY_ANDROID)
            onPointerClick?.Invoke();
#else
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onPointerClick?.Invoke();
            }
            else
            {
                onPointerOptionClick?.Invoke();
            }
#endif
        }

        public override void OnPointerEnter(PointerEventData eventData)
        {
            base.OnPointerEnter(eventData);
            onPointerEnter?.Invoke();
        }

        public override void OnPointerExit(PointerEventData eventData)
        {
            base.OnPointerExit(eventData);
            onPointerExit?.Invoke();
        }

        public override void OnPointerDown(PointerEventData eventData)
        {
#if (UNITY_IOS || UNITY_ANDROID)
            if (longPointerCoroutine != null)
            {
                StopCoroutine(longPointerCoroutine);
            }

            longPointerCoroutine = StartCoroutine(LongPointerDownCoroutine(eventData));
#endif

            base.OnPointerDown(eventData);
        }

#if (UNITY_IOS || UNITY_ANDROID)
        IEnumerator LongPointerDownCoroutine(PointerEventData eventData)
        {
            var pressTime = Time.unscaledTime;
            var pressPosition = eventData.position;

            while (Time.unscaledTime < pressTime + 1.0f)
            {
                if ((eventData.position - pressPosition).magnitude > 10.0f)
                {
                    longPointerCoroutine = null;
                    yield break;
                }

                yield return null;
            }

            onPointerOptionClick?.Invoke();
            longPointerCoroutine = null;
            yield break;
        }
#endif
    }
}
