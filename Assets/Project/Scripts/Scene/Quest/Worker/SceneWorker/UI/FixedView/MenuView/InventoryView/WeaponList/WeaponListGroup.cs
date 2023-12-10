using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class WeaponListGroup : MonoBehaviour
    {
        [SerializeField] GridLayoutGroup gridLayoutGroup;

        public void UpdateLayout(Vector2 localPosition, int cellCount)
        {
            gridLayoutGroup.constraintCount = (int)Mathf.Floor(Mathf.Sqrt(cellCount));

            gridLayoutGroup.CalculateLayoutInputHorizontal();
            gridLayoutGroup.CalculateLayoutInputVertical();
            gridLayoutGroup.SetLayoutHorizontal();
            gridLayoutGroup.SetLayoutVertical();

            var rectTransform = (RectTransform)transform;
            var parentRectTransform = (RectTransform)rectTransform.parent;

            var scale = parentRectTransform.rect.size * 0.5f;
            rectTransform.localPosition = new Vector3(scale.x * localPosition.x, scale.y * localPosition.y, 0);
            rectTransform.sizeDelta = new Vector2(gridLayoutGroup.preferredWidth, gridLayoutGroup.preferredHeight);
        }
    }
}
