using System;
using UnityEngine;
using UnityEngine.UI;

namespace AloneSpace
{
    public class AreaDataCell : MonoBehaviour
    {
        [SerializeField] ParticleSystem particleSystem;
        [SerializeField] Button button;

        AreaData areaData;
        bool isCurrentArea;
        Action<AreaData> onClick;

        public void Awake()
        {
            button.onClick.AddListener(() => onClick(areaData));
        }

        public void Apply(AreaData areaData, bool isCurrentArea, Action<AreaData> onClick)
        {
            this.areaData = areaData;
            this.isCurrentArea = isCurrentArea;
            this.onClick = onClick;

            this.isCurrentArea = false;
            
            gameObject.name = $"Area {areaData.AreaId}";
        }

        public void UpdatePosition(Vector3? position)
        {
            gameObject.SetActive(position.HasValue && !isCurrentArea);
            if (!position.HasValue || isCurrentArea)
            {
                return;
            }

            (transform as RectTransform).localPosition = position.Value;
        }

        static Color GetColor(int areaId, int observeAreaId)
        {
            if (observeAreaId == areaId)
            {
                return new Color(0.2f, 0.4f, 0.2f);
            }

            return new Color(0.5f, 0.5f, 0.5f, 0.1f);
        }
    }
}
