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

        public void ApplyDamage(float damage, Vector3 screenPosition)
        {
            text.text = $"{damage:0}";
            rectTransform.localPosition = screenPosition;
            
            gameObject.SetActive(true);

            StartCoroutine(ShowDamage());
        }

        IEnumerator ShowDamage()
        {
            yield return new WaitForSeconds(1.5f);
            gameObject.SetActive(false);
        }
    }
}