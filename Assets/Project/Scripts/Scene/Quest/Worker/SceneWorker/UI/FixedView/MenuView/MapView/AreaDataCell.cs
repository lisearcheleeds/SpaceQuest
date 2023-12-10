using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace.UI
{
    public class AreaDataCell : MonoBehaviour
    {
        [SerializeField] Image image;
        [SerializeField] Button button;

        AreaData areaData;
        Action<AreaData> onClick;

        public void Awake()
        {
            button.onClick.AddListener(() => onClick(areaData));
        }

        public void Apply(AreaData areaData, bool isCurrentArea, Vector3 position, Action<AreaData> onClick)
        {
            this.areaData = areaData;
            this.onClick = onClick;

            gameObject.name = $"Area {areaData.AreaId}";
            transform.localPosition = position;
            image.color = GetColor(isCurrentArea);
        }

        static Color GetColor(bool isCurrentArea)
        {
            if (isCurrentArea)
            {
                return new Color(0.4f, 0.6f, 0.4f);
            }

            return Color.white;
        }
    }
}
