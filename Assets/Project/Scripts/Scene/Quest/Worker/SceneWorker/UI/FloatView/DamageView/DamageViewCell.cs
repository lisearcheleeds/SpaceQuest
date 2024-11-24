using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class DamageViewCell : MonoBehaviour
    {
        [SerializeField] Text text;
        [SerializeField] RectTransform rectTransform;

        public bool IsActive => gameObject.activeSelf;

        Vector3 position;
        RectTransform canvasParent;
        Vector3 randomOffset;
        
        public void ApplyDamage(float damage, Vector3 position, RectTransform canvasParent)
        {
            text.text = $"{damage:0}";
            
            gameObject.SetActive(true);
            this.position = position;
            this.canvasParent = canvasParent;
            randomOffset = Random.insideUnitSphere * 20.0f;

            StartCoroutine(ShowDamage());
        }

        void Update()
        {
            var canvasPoint = MessageBus.Instance.Util.GetWorldToCanvasPoint.Unicast(
                CameraType.NearCamera,
                position,
                canvasParent);

            rectTransform.localPosition = (canvasPoint ?? Vector3.zero) + randomOffset;
        }

        IEnumerator ShowDamage()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
    }
}